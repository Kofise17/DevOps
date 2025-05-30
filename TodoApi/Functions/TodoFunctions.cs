using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Functions
{
    public class TodoFunctions
    {
        private readonly TodoContext _context;

        public TodoFunctions(TodoContext context)
        {
            _context = context;
        }

        [Function("GetAllTodos")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequestData req,
        FunctionContext executionContext)
        {
            var todos = await _context.TodoItems.ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(todos);

            return response;
        }

        [Function("GetTodoById")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{id:int}")] HttpRequestData req,
        FunctionContext executionContext,
        int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(todo);

            return response;
        }

        [Function("CreateTodo")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "todos")] HttpRequestData req)
        {
            var newTodo = await req.ReadFromJsonAsync<TodoItem>();
            if (newTodo is null)
                return req.CreateResponse(HttpStatusCode.BadRequest);

            _context.TodoItems.Add(newTodo);
            await _context.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(newTodo);
            return response;
            
        }
    }
}
