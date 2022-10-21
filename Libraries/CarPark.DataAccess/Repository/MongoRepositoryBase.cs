using CarPark.Core.Models;
using CarPark.Core.Repository.Abstract;
using CarPark.DataAccess.Context;
using CarPark.DataAccess.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CarPark.DataAccess.Repository
{
    public class MongoRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepositoryBase(IOptions<MongoConnection> settings)
        {
            _mongoDbContext = new MongoDbContext(settings);
            _collection = _mongoDbContext.GetCollection<TEntity>();
        }

        public GetManyResult<TEntity> AsQueryable()
        {
            var result = new GetManyResult<TEntity>();

            try
            {
                var data = _collection.AsQueryable().ToEnumerable();
                if (data != null)
                    result.Result = data;
            }
            catch (Exception exp)
            {
                result.Message = $"AsQueryable {exp.Message}";
                result.Success = false;
                result.Result = Enumerable.Empty<TEntity>();
            }

            return result;
        }

        public async Task<GetManyResult<TEntity>> AsQueryableAsync()
        {
            var result = new GetManyResult<TEntity>();

            try
            {
                var data = await _collection.AsQueryable().ToListAsync();
                if (data != null)
                    result.Result = data;
            }
            catch (Exception exp)
            {
                result.Message = $"AsQueryableAsync {exp.Message}";
                result.Success = false;
                result.Result = Enumerable.Empty<TEntity>();
            }

            return result;
        }

        public GetManyResult<TEntity> FilterBy(Expression<Func<TEntity, bool>> filter)
        {
            var result = new GetManyResult<TEntity>();

            try
            {
                var data = _collection.Find(filter).ToEnumerable();
                if (data != null)
                    result.Result = data;
            }
            catch (Exception exp)
            {
                result.Message = $"FilterBy {exp.Message}";
                result.Success = false;
                result.Result = Enumerable.Empty<TEntity>();
            }

            return result;
        }

        public async Task<GetManyResult<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> filter)
        {
            var result = new GetManyResult<TEntity>();

            try
            {
                var data = await _collection.FindAsync(filter);
                if (data != null)
                    result.Result = data.ToEnumerable();
            }
            catch (Exception exp)
            {
                result.Message = $"FilterByAsync {exp.Message}";
                result.Success = false;
                result.Result = Enumerable.Empty<TEntity>();
            }

            return result;
        }

        public GetOneResult<TEntity> GetById(string id)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var data = _collection.Find(filter).FirstOrDefault();

                if (data != null)
                    result.Entity = data;
            }
            catch (Exception exp)
            {
                result.Message = $"GetById {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public async Task<GetOneResult<TEntity>> GetByIdAsync(string id)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var data = await _collection.Find(filter).FirstOrDefaultAsync();

                if (data != null)
                    result.Entity = data;
            }
            catch (Exception exp)
            {
                result.Message = $"GetByIdAsync {exp.Message}";
                result.Success = false;
            }

            return result;
        }



        public GetOneResult<TEntity> InsertOne(TEntity entity)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                _collection.InsertOne(entity);
                result.Entity = entity;
            }
            catch (Exception exp)
            {
                result.Message = $"InsertOne {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public async Task<GetOneResult<TEntity>> InsertOneAsync(TEntity entity)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                await _collection.InsertOneAsync(entity);
                result.Entity = entity;
            }
            catch (Exception exp)
            {
                result.Message = $"InsertOneAsync {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public GetManyResult<TEntity> InsertMany(ICollection<TEntity> entities)
        {
            var result = new GetManyResult<TEntity>();

            try
            {
                _collection.InsertMany(entities);
                result.Result = entities;
            }
            catch (Exception exp)
            {
                result.Message = $"InsertMany {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public async Task<GetManyResult<TEntity>> InsertManyAsync(ICollection<TEntity> entities)
        {
            var result = new GetManyResult<TEntity>();

            try
            {
                await _collection.InsertManyAsync(entities);
                result.Result = entities;
            }
            catch (Exception exp)
            {
                result.Message = $"InsertManyAsync {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public GetOneResult<TEntity> ReplaceOne(TEntity entity, string id)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var updatedDocument = _collection.ReplaceOne(filter, entity);

                result.Entity = entity;
            }
            catch (Exception exp)
            {
                result.Message = $"ReplaceOne {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public async Task<GetOneResult<TEntity>> ReplaceOneAsync(TEntity entity, string id)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var updatedDocument = await _collection.ReplaceOneAsync(filter, entity);

                result.Entity = entity;
            }
            catch (Exception exp)
            {
                result.Message = $"ReplaceOne {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public GetOneResult<TEntity> DeleteOne(Expression<Func<TEntity, bool>> filter)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var deletedDocument = _collection.FindOneAndDelete(filter);

                if (deletedDocument != null)
                    result.Entity = deletedDocument;
            }
            catch (Exception exp)
            {
                result.Message = $"DeleteOne {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public async Task<GetOneResult<TEntity>> DeleteOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var deletedDocument = await _collection.FindOneAndDeleteAsync(filter);

                if (deletedDocument != null)
                    result.Entity = deletedDocument;
            }
            catch (Exception exp)
            {
                result.Message = $"DeleteOneAsync {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public GetOneResult<TEntity> DeleteById(string id)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var data = _collection.FindOneAndDelete(filter);

                if (data != null)
                    result.Entity = data;
            }
            catch (Exception exp)
            {
                result.Message = $"DeleteById {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public async Task<GetOneResult<TEntity>> DeleteByIdAsync(string id)
        {
            var result = new GetOneResult<TEntity>();

            try
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var data = await _collection.FindOneAndDeleteAsync(filter);

                if (data != null)
                    result.Entity = data;
            }
            catch (Exception exp)
            {
                result.Message = $"DeleteByIdAsync {exp.Message}";
                result.Success = false;
            }

            return result;
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            _collection.DeleteMany(filter);
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            await _collection.DeleteManyAsync(filter);
        }
    }
}
