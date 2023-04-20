using MediatR;
using System;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Name { get; init; }
    }
}