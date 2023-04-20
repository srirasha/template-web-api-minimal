using Application._Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IApplicationDbContext _context;

        public GetUserByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.Id, out Guid userId))
            {
                return await _context.Users.FirstOrDefaultAsync(usr => usr.Id == userId, cancellationToken);
            }
            else
            {
                return null;
            };
        }
    }
}