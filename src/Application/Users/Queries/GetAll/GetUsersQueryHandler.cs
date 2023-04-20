using Application._Common.Interfaces;
using Application._Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetAll
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<User>>
    {
        private readonly IApplicationDbContext _context;

        public GetUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await PaginatedList<User>.Create(_context.Users, request.PageNumber, request.PageSize);
        }
    }
}