using System.Text.Json;
using PANDA.Api.Infrastructure;
using PANDA.Domain.Entities;

namespace PANDA.Api.Services.Audit;

/// <summary>
/// Service responsible for logging create, update, and delete actions to the audit log.
/// </summary>
public class AuditService : IAuditService
{
    /// <summary>
    /// Represents the database context used for interacting with the underlying database.
    /// This context provides access to the application's entities and is used to perform
    /// database operations such as querying, inserting, updating, and deleting data.
    /// </summary>
    private readonly PandaDbContext _context;

    /// <summary>
    /// Provides access to the current HTTP context, allowing the service to retrieve information
    /// about the current user, request, and other context-specific details.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// Provides methods for performing audit logging within the system.
    /// Responsible for tracking create, update, and delete actions performed on entities.
    /// This service interacts with the database to persist audit logs for later review
    /// and provides utility for differentiating between entities and capturing
    /// user actions within the application.
    public AuditService(PandaDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _httpContextAccessor = accessor;
    }

    /// <summary>
    /// Retrieves the username of the currently authenticated user, if available.
    /// </summary>
    /// <returns>
    /// The username of the currently authenticated user. If the username is unavailable, returns "Unknown".
    /// </returns>
    private string GetUsername() =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

    /// <summary>
    /// Retrieves the name of the entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>The name of the entity type as a string.</returns>
    private string GetEntityName<TEntity>() => typeof(TEntity).Name;

    /// Retrieves the value of the "Id" property from the specified entity object.
    /// If the "Id" property does not exist or its value is null, "N/A" is returned.
    /// <param name="entity">The entity object from which to retrieve the "Id" property value.</param>
    /// <returns>The value of the "Id" property as a string, or "N/A" if the property is not found or its value is null.
    private string GetEntityId(object entity)
    {
        var idProp = entity.GetType().GetProperty("Id");
        return idProp?.GetValue(entity)?.ToString() ?? "N/A";
    }

    /// <summary>
    /// Logs the creation of a new entity in the audit log with details about the created entity and user responsible.
    /// </summary>
    /// <typeparam name="TDto">The type of the data transfer object (DTO) containing the details of the created entity.</typeparam>
    /// <typeparam name="TEntity">The type of the entity that was created.</typeparam>
    /// <param name="input">The DTO containing the data used to create the entity.</param>
    /// <param name="resultEntity">The entity instance that was created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LogCreateAsync<TDto, TEntity>(TDto input, TEntity resultEntity)
    {
        var log = new AuditLog
        {
            Username = GetUsername(),
            Action = "Create",
            Entity = GetEntityName<TEntity>(),
            EntityId = GetEntityId(resultEntity),
            Changes = JsonSerializer.Serialize(input),
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    /// Logs an update action performed on an entity, capturing the changes made during the update.
    /// <param name="input">The DTO representing the new data for the entity.</param>
    /// <param name="original">The original entity prior to the update.</param>
    /// <param name="updated">The updated entity after the changes have been applied.</param>
    /// <typeparam name="TDto">The type of the Data Transfer Object containing the new data.</typeparam>
    /// <typeparam name="TEntity">The type of the entity being updated.</typeparam>
    /// <return>A task that represents the asynchronous operation.</return>
    public async Task LogUpdateAsync<TDto, TEntity>(TDto input, TEntity original, TEntity updated)
    {
        var changes = new
        {
            Original = original,
            Updated = input
        };

        var log = new AuditLog
        {
            Username = GetUsername(),
            Action = "Update",
            Entity = GetEntityName<TEntity>(),
            EntityId = GetEntityId(original),
            Changes = JsonSerializer.Serialize(changes),
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    /// Logs the deletion of an entity to the audit log.
    /// <typeparam name="TEntity">The type of the entity being deleted.</typeparam>
    /// <param name="entity">The entity instance that was deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LogDeleteAsync<TEntity>(TEntity entity)
    {
        var log = new AuditLog
        {
            Username = GetUsername(),
            Action = "Delete",
            Entity = GetEntityName<TEntity>(),
            EntityId = GetEntityId(entity),
            Changes = JsonSerializer.Serialize(entity),
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
