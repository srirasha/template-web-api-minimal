using Domain.Entities;
using MediatR;

namespace Application.Users.Queries.GetById
{
    public record GetUserByIdQuery(string Id) : IRequest<User>;
}