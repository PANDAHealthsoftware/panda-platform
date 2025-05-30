namespace PANDA.Api.Services.Audit;

public interface IAuditService
{
    Task LogCreateAsync<TDto, TEntity>(TDto input, TEntity resultEntity);
    Task LogUpdateAsync<TDto, TEntity>(TDto input, TEntity originalEntity, TEntity updatedEntity);
    Task LogDeleteAsync<TEntity>(TEntity entity);
}
