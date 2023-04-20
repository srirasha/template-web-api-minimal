using Application._Common.Interfaces;
using Application._Common.Models;
using Application.Users.Commands.Create;
using Application.Users.Queries.GetAll;
using Application.Users.Queries.GetById;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using Web.MyAPI.Minimal.Middlewares;
using Web.MyAPI.Models.Requests;
using Web.MyAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionsHandlerMiddleware>();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //database initialization
    if (!app.Configuration.GetValue<bool>("UseInMemoryDatabase"))
    {
        using IServiceScope scope = app.Services.CreateScope();
        ApplicationDbContextInitialiser dbInitialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await dbInitialiser.Initialise();
    }
}

app.MapGet("/healthcheck/hello", [EndpointSummary("Say hello to the API!")]
() =>
{
    return Results.Ok("hello!");
})
.WithOpenApi();

app.MapPost("v1/users", [EndpointSummary("Create a user")]
async (CreateUserRequest request, IMediator mediator, CancellationToken cancellationToken) =>
{
    Guid newUserId = await mediator.Send(new CreateUserCommand() { Name = request.Name }, cancellationToken);

    return Results.Created($"/users/{newUserId}", newUserId);
})
.Produces<Guid>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.WithOpenApi();

app.MapGet("v1/users/{id}", [EndpointSummary("Get a user by id")]
async (string id, IMediator mediator, CancellationToken cancellationToken) =>
{
    User user = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);

    return user != null ? Results.Ok(user) : Results.NotFound();
})
.Produces<User>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapGet("v1/users", [EndpointSummary("Get all users")]
async (int pageNumber, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
{
    return Results.Ok(await mediator.Send(new GetUsersQuery() { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken));
})
.Produces<IEnumerable<PaginatedList<User>>>(StatusCodes.Status200OK)
.WithOpenApi();

app.Run();