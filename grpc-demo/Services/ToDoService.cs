using Grpc.Core;
using grpc_demo.Data;
using grpc_demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using ToDoGrpc;
using ToDoGrpc.Services;

namespace grpc_demo.Services
{
    public class ToDoService : ToDoIt.ToDoItBase
    {
        private ILogger<ToDoService> _logger { get; set; }
        private readonly AppDbContext _dbContext;
        public ToDoService(AppDbContext dbContext, ILogger<ToDoService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
        {
            if (request.Title == string.Empty || request?.Description == string.Empty)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));
            }

            var toDoItem = new ToDoItem
            {
                Title = request?.Title,
                Description = request?.Description
            };

            var result = _dbContext.ToDoItems.Add(toDoItem);
            _dbContext.SaveChanges();

            return Task.FromResult(new CreateToDoResponse
            {
                Id = result.Entity.Id
            });
        }

        public override Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
        {
            if (request?.Id == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));
            }

            var result = _dbContext.ToDoItems.Where(x=>x.Id == request.Id).FirstOrDefault();

            return Task.FromResult(new ReadToDoResponse
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                ToDoStatus = result.ToDoStatus
            });
        }

        public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
        {
            if (request.Id <= 0 || request.Title == string.Empty || request.Description == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            var toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id);

            if (toDoItem == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"No Task with Id {request.Id}"));

            toDoItem.Title = request.Title;
            toDoItem.Description = request.Description;
            toDoItem.ToDoStatus = request.ToDoStatus;

            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(new UpdateToDoResponse
            {
                Id = toDoItem.Id
            });
        }
    }
}
