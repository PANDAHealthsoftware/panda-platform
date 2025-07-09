using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using PANDA.Api.Services.Audit;
using PANDA.Domain.Enums;
using PANDA.Shared.DTOs.User;
using PANDA.Shared.Helpers;

namespace PANDA.Api.Services.User;

public class UserService : IUserService
{
    private readonly PandaDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;

    public UserService(PandaDbContext context, IMapper mapper, IAuditService auditService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var allUsers = await _context.Users
            .Include(u => u.Role)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(allUsers);
    }

    // CREATE USER
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            throw new InvalidOperationException("Username already exists.");

        var user = _mapper.Map<Models.User>(dto);
        user.PasswordHash = PasswordHelper.HashPassword(dto.Password);
        user.Status = dto.Status;
        user.RoleId = dto.RoleId;
        user.LastModified = DateTime.UtcNow;

        _context.Users.Add(user);
        await _auditService.LogCreateAsync(dto, user);
        await _context.SaveChangesAsync();

        user = await _context.Users
            .Include(u => u.Role)
            .FirstAsync(u => u.Id == user.Id);

        return _mapper.Map<UserDto>(user);
    }

    // UPDATE USER
    public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == dto.Id);

        if (user == null || user.IsDeleted)
            return false;

        var original = JsonConvert.DeserializeObject<Models.User>(
            JsonConvert.SerializeObject(user));

        user.Username = dto.Username;
        user.Status = dto.Status;
        user.LastModified = DateTime.UtcNow;

        user.RoleId = dto.RoleId; // ✅ update direct FK

        await _auditService.LogUpdateAsync(dto, original, user);
        await _context.SaveChangesAsync();
        return true;
    }

    // SOFT DELETE
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null || user.IsDeleted)
            return false;

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<RoleType?> GetUserRoleEnumAsync(int id)
    {
        var roleId = await _context.Users
            .Where(u => u.Id == id)
            .Select(u => (RoleType?)u.RoleId)
            .FirstOrDefaultAsync();

        return roleId;
    }

    public async Task<IEnumerable<Role>> GetUserRolesByUserIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user != null && user.Role != null
            ? new List<Role> { user.Role }
            : new List<Role>();
    }

    public async Task<Models.User?> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && !u.IsDeleted);

        if (user == null)
            return null;

        return PasswordHelper.VerifyPassword(password, user.PasswordHash) ? user : null;
    }
}
