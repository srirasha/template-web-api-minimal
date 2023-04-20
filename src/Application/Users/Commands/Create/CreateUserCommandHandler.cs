using Application._Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User newUser = new() { Name = request.Name };

            await _context.Users.AddAsync(newUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newUser.Id;
        }
    }
}