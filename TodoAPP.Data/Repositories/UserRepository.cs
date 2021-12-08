using System;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Query;
using TodoAPP.Core.Entity;
using TodoAPP.Core.Repositories;
using TodoAPP.Infrastructure.Model;

namespace TodoAPP.Data.Repositories
{
    public class UserRepository : CouchbaseRepository<User>, IUserRepository
    {
        public UserRepository(ITodoBucketProvider bucketProvider, IClusterProvider clusterProvider)
            : base(bucketProvider, clusterProvider)
        {
        }

        public Result Save(User newUser)
        {
            var result = new Result();
            // if (GetByMail(newUser.Email).Result != null)
            //     return result.AddError("This email is already being used by another user.");

            Create(newUser);
            result.Data = newUser;
            return result;
        }

        public async Task<User> GetByMail(string mailAddress)
        {
            var bucket = await _bucketProvider.GetBucketAsync().ConfigureAwait(false);
            var cluster = await _clusterProvider.GetClusterAsync().ConfigureAwait(false);
            
            var query = $@"SELECT t.* 
                FROM {bucket.Name} as t 
                WHERE type = '{Type}' AND email = '{mailAddress}'
                LIMIT 1;";
            var options = new QueryOptions();

            if (mailAddress != null && !string.IsNullOrEmpty(mailAddress))
            {
                options.Parameter(mailAddress);
            }

            var queryResult = cluster.QueryAsync<User>(query, options);
            return await queryResult.Result.FirstAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await Get(id);
        }

        public async Task<Result> SaveTodo(Guid userId, ToDoListItem newTodo)
        {
            var user = await Get(userId);
            user.Projects.Add(newTodo);
            await Update(user);
            return new Result(newTodo);
        }

        public async Task<BaseResult> UpdateTodo(Guid userId, ToDoListItem currentTodo)
        {
            var toReturn = new BaseResult();
            var user = await Get(userId);
            var index = user.Projects.FindIndex(x => x.Id.Equals(currentTodo.Id));
            if (index == -1)
                return toReturn.AddError("Project Not Found!");

            user.Projects[index] = currentTodo;
            await Update(user);
            return toReturn.AddSuccess();
        }

        public async Task<BaseResult> DeleteTodo(Guid userId, Guid todoId)
        {
            var toReturn = new BaseResult();
            var user = await Get(userId);
            var index = user.Projects.FindIndex(x => x.Id.Equals(todoId));
            if (index == -1)
                return toReturn.AddError("Project Not Found!");

            user.Projects.RemoveAt(index);
            await Update(user);
            return toReturn.AddSuccess();
        }

        public async Task<Result> SaveTask(Guid userId, Guid todoId, TaskItem newTask)
        {
            var user = await Get(userId);
            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(todoId));
            if (project == null)
                return new Result().AddError("Project Not Found!");
            project.Tasks.Add(newTask);
            await Update(user);
            return new Result(newTask);
        }

        public async Task<BaseResult> UpdateTask(Guid userId, Guid todoId, TaskItem task)
        {
            var toReturn = new BaseResult();
            var user = await Get(userId);
            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(todoId));
            if (project == null)
                return toReturn.AddError("Project Not Found!");

            var index = project.Tasks.FindIndex(x => x.Id.Equals(task.Id));
            if (index == -1)
                return toReturn.AddError("Task Not Found!");

            project.Tasks[index] = task;
            await Update(user);
            return toReturn.AddSuccess();
        }

        public async Task<BaseResult> DeleteTask(Guid userId, Guid todoId, Guid taskId)
        {
            var toReturn = new BaseResult();
            var user = await Get(userId);
            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(todoId));
            if (project == null)
                return toReturn.AddError("Project Not Found!");

            var index = project.Tasks.FindIndex(x => x.Id.Equals(taskId));
            if (index == -1)
                return toReturn.AddError("Task Not Found!");

            project.Tasks.RemoveAt(index);
            await Update(user);
            return toReturn.AddSuccess();
        }
    }
}