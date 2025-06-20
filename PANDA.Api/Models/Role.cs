﻿namespace PANDA.Api.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}