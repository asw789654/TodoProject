using Users.BL.DTO;

namespace Users.Services
{
    public interface IUserService
    {
        public Task<IReadOnlyCollection<GetUserDto>> GetListAsync(
            int? offset, 
            string labelFreeText, 
            int? limit = 10, 
            CancellationToken cancellationToken = default);
        public Task<GetUserDto?> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        public Task<GetUserDto?> AddToListAsync(AddUserDto user, CancellationToken cancellationToken = default);

        public Task<GetUserDto> UpdateAsync(UpdateUserDto user, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(RemoveUserDto user, CancellationToken cancellationToken = default);
        public Task<GetUserDto> UpdatePasswordAsync(UpdateUserPasswordDto user, CancellationToken cancellationToken = default);
        
    }
}
