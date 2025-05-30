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

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var allUsers = await _context.Users.ToListAsync();

        return _mapper.Map<List<UserDto>>(allUsers);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
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