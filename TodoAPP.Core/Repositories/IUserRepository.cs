using System;
using System.Threading.Tasks;
using TodoAPP.Core.Entity;
using TodoAPP.Infrastructure.Model;

namespace TodoAPP.Core.Repositories
{
    public interface IUserRepository
    {
        Result Save(User newUser);
        Task<User> GetByMail(string mailAddress);
        Task<User> GetById(Guid id);

        Task<Result> SaveTodo(Guid userId, ToDoListItem newTodo);
        Task<BaseResult> UpdateTodo(Guid userId, ToDoListItem currentTodo);
        Task<BaseResult> DeleteTodo(Guid userId, Guid todoId);

        Task<Result> SaveTask(Guid userId, Guid todoId, TaskItem newTask);
        Task<BaseResult> UpdateTask(Guid userId, Guid todoId, TaskItem task);
        Task<BaseResult> DeleteTask(Guid userId, Guid todoId, Guid taskId);
    }
}