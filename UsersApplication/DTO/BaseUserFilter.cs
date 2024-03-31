using MediatR;

namespace Users.Application.DTO
{
    public class BaseUserFilter : IRequest<int>
    {
        public string? NameFreeText { get; set; }
    }
}
