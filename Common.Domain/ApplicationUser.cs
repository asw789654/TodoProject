namespace Common.Domain;

public class ApplicationUser
{
    public int Id { get; set; }
    public string Name { get; set; } = default;
    public string PasswordHash { get; set; } = default;
    public IEnumerable<ApplicationUserApplicationRole>  Roles { get; set; } = default;
    public ICollection<Todo> Todos { get; set; } = default;
}
