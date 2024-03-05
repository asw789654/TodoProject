using Common.Domain;
using Users.BL.DTO;

namespace Users.Services
{
    public interface IUserService
    {
        IReadOnlyCollection<User> GetList(int? offset, string labelFreeText, int? limit = 10);
        public User? GetById(int id);
        public User? AddToList(AddUserDto todo);

        public User Update(UpdateUserDto todo);

        public bool Delete(RemoveUserDto user);

    }
}
