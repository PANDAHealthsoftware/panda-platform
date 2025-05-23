﻿using PANDA.Domain.Entities;

namespace PANDA.Api.Models;

public class User: AuditableEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}