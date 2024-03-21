namespace Users.BL.DTO
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = default;
        JwtTokenDto jwtTokenDto { get; set; }
    }
}
