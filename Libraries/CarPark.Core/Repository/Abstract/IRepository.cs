using CarPark.Core.Models;
using System.Linq.Expressions;

namespace CarPark.Core.Repository.Abstract
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        GetManyResult<TEntity> GetAll();
        Task<GetManyResult<TEntity>> GetAllAsync();

        GetManyResult<TEntity> FilterBy(Expression<Func<TEntity, bool>> filter);
        Task<GetManyResult<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> filter);

        GetOneResult<TEntity> GetById(string id, string idType = "object");
        Task<GetOneResult<TEntity>> GetByIdAsync(string id, string idType = "object");

        GetOneResult<TEntity> InsertOne(TEntity entity);
        Task<GetOneResult<TEntity>> InsertOneAsync(TEntity entity);

        GetManyResult<TEntity> InsertMany(ICollection<TEntity> entities);
        Task<GetManyResult<TEntity>> InsertManyAsync(ICollection<TEntity> entities);

        GetOneResult<TEntity> ReplaceOne(TEntity entity, string id, string idType = "object");
        Task<GetOneResult<TEntity>> ReplaceOneAsync(TEntity entity, string id, string idType = "object");

        GetOneResult<TEntity> DeleteOne(Expression<Func<TEntity, bool>> filter);
        Task<GetOneResult<TEntity>> DeleteOneAsync(Expression<Func<TEntity, bool>> filter);

        GetOneResult<TEntity> DeleteById(string id, string idType = "object");
        Task<GetOneResult<TEntity>> DeleteByIdAsync(string id, string idType = "object");

        void DeleteMany(Expression<Func<TEntity, bool>> filter);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter);
    }
}
