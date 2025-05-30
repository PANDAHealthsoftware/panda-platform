using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Helpers;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using PANDA.Domain.Enums;
using PANDA.Shared.DTOs.User;

namespace PANDA.Api.Services.User;

public class UserService : IUserService
{
    private readonly PandaDbContext _context;
    private readonly IMapper _mapper;

    public UserService(PandaDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var allUsers = await _context.Users.ToListAsync();

        return _mapper.Map<List<UserDto>>(allUsers);
    }
    
    // CREATE USER
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        // Username must be unique!
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            throw new InvalidOperationException("Username already exists.");

        // Hash password
        var user = _mapper.Map<Models.User>(dto);
        user.PasswordHash = PasswordHelper.HashPassword(dto.Password);
        user.UserRoles = dto.RoleIds.Select(roleId => new UserRole { RoleId = roleId }).ToList();
        user.Status = dto.Status;
        user.LastModified = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Return with role names
        user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstAsync(u => u.Id == user.Id);

        return _mapper.Map<UserDto>(user);
    }

    // UPDATE USER
    public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == dto.Id);

        if (user == null || user.IsDeleted)
            return false;

        user.Username = dto.Username;
        user.Status = dto.Status;
        user.LastModified = DateTime.UtcNow;

        // Update roles (remove old, add new)
        user.UserRoles.Clear();
        foreach (var roleId in dto.RoleIds)
            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId });

        await _context.SaveChangesAsync();
        return true;
    }

    // SOFT DELETE USER
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
        var user = await _context.Users.FindAsync(id);
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

    public async Task<List<Role>> GetUserRolesCollectionByUserIdAsync(int id)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == id)
            .Select(ur => ur.Role)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Role>> GetUserRolesByUserIdAsync(int id)
    {
        return await _context.Roles
            .Where(r => r.Users.Any(u => u.Id == id))
            .ToListAsync();
    }
    public async Task<Models.User?> ValidateCredentialsAsync(string username, string password)
    {
        // Find a user by username (case-insensitive)
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

        if (user == null)
            return null;

        // Check password (hashed)
        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            return null;

        return user;
    }
}