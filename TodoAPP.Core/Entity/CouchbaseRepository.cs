using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Extensions.DependencyInjection;
using System.Linq;
using System.Runtime.CompilerServices;
using TodoAPP.Core.Repositories;


namespace TodoAPP.Core.Entity
{
    public abstract class CouchbaseRepository<T>
        where T : CouchbaseEntity<T>
    {
        protected readonly ITodoBucketProvider _bucketProvider;
        protected readonly IClusterProvider _clusterProvider;

        protected CouchbaseRepository(ITodoBucketProvider bucketProvider, IClusterProvider clusterProvider)
        {
            _bucketProvider = bucketProvider;
            _clusterProvider = clusterProvider;
        }

        protected async Task<T> Get(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new Exception("Error in input data, Id is required!");
            }

            var bucket = await _bucketProvider.GetBucketAsync().ConfigureAwait(false);
            var collection = await bucket.DefaultCollectionAsync();
            var getResult = await collection.GetAsync(CreateKey(id));
            return getResult.ContentAs<T>();
        }

        protected async Task<IEnumerable<T>> GetAll(int limit = 10)
        {
            var cluster = await _clusterProvider.GetClusterAsync().ConfigureAwait(false);
            var bucket = await _bucketProvider.GetBucketAsync().ConfigureAwait(false);

            var query = $@"SELECT t.* 
                FROM {bucket.Name} as t 
                WHERE type = '{Type}' 
                LIMIT {limit};";

            var queryResult = cluster.QueryAsync<T>(query);
            return await queryResult.Result.Rows.ToListAsync<T>();
        }

        protected async Task<T> Create(T item)
        {
            var bucket = await _bucketProvider.GetBucketAsync().ConfigureAwait(false);
            var collection = await bucket.DefaultCollectionAsync();
            await collection.InsertAsync<T>(CreateKey(item.Id), item).ConfigureAwait(false);
            return item;
        }

        protected async Task<T> Update(T item)
        {
            if (item.Id == Guid.Empty)
            {
                throw new Exception("Error in input data, Id is required!");
            }


            var bucket = await _bucketProvider.GetBucketAsync().ConfigureAwait(false);
            var collection = await bucket.DefaultCollectionAsync();
            await collection.ReplaceAsync<T>(CreateKey(item.Id), item);

            return item;
        }

        protected async Task Delete(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new Exception("Error in input data, Id is required!");
            }

            var bucket = await _bucketProvider.GetBucketAsync().ConfigureAwait(false);
            var collection = await bucket.DefaultCollectionAsync();
            await collection.RemoveAsync(CreateKey(id));
        }

        private string CreateKey(Guid id) => $"{Type}::{id}";
        protected string Type => typeof(T).Name.ToLower();
    }
}