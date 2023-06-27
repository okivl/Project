using Project.Core.Options.Params.CreateUpdate;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;

namespace Project.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll(UserSort sortBy, Pagination pagination);
        Task<User> Get(Guid Id);
        Task<User> GetUserInfo();
        Task Create(AdminUserCreate user);
        Task Update(Guid Id, AdminUserUpdate user);
        Task UpdateUser(BaseUserUpdate userUpdate);
        Task UpdatePassword(Guid Id, BaseUser password);
        Task UpdateUserPassword(BaseUser password);
        Task Delete(Guid Id);
    }
}
