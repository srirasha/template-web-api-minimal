using Application._Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Users.Queries.GetAll
{
    public class GetUsersQuery : IRequest<PaginatedList<User>>
    {
        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = 10;
    }
}