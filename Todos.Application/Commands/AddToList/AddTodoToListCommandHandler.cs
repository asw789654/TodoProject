using AutoMapper;
using Common.Api.Services;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using MediatR;

namespace Todos.Application.Commands.AddToList;

public class AddTodoToListCommandHandler : IRequestHandler<AddTodoToListCommand, Todo?>
{
    private readonly IRepository<Todo> _todos;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    public AddTodoToListCommandHandler(IRepository<Todo> todos,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _todos = todos;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }


    public async Task<Todo?> Handle(AddTodoToListCommand request, CancellationToken cancellationToken = default)
    {
        request.Id = _todos.CountAsync(cancellationToken: cancellationToken).Result + 1;
        var todoEntity = _mapper.Map<Todo>(request);
        todoEntity.OwnerId = _currentUserService.CurrentUserId();

        return await _todos.AddAsync(todoEntity, cancellationToken);
    }
}
