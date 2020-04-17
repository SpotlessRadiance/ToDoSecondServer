using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecondServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondServer.Abstractions;
using Newtonsoft.Json;
using System.Net.Http;

namespace SecondServer.Controllers
{
    [Produces("application/json")]
    [Route("api/todostatus")]
    [ApiController]
    public class TodoStatusController : ControllerBase
    {
        private IItemsRepository repositoryItems;
        private readonly IHttpClientFactory clientFactory;
        public TodoStatusController(IItemsRepository repo, IHttpClientFactory factory)
         { 
            repositoryItems = repo;
           clientFactory = factory;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostTodoItem(ToDoItem item)
        {
            bool completed = await repositoryItems.AddItem(item);
            if (!completed)
            {
                return StatusCode(500);
            }
            Dictionary<string, long> d = new Dictionary<string, long> { { "id", item.ID } };
            return Ok(d);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDoItem>> DeleteToDoItem(long id)
        {
            bool completed = await repositoryItems.DeleteItem(id);
            if (!completed)
                return BadRequest();
            return StatusCode(200);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetTodoItem(long id)
        {
            var todoItem = await repositoryItems.GetItem(id);
            if (todoItem == null)
            {
                return StatusCode(400);
            }
            var response = new
            {
                status = todoItem.IsCompleted,
                recent_change = todoItem.RecentUpdate
            };
            return Ok(response);
        }

        [HttpGet("history/{id}")]
        public async Task<ActionResult> GetHistoryResult(long id)
        {
            var item = await repositoryItems.GetItem(id);
            if (item == null)
            {
                return StatusCode(400);
            }
             var changes = await repositoryItems.GetHistoryAsync(id);
            return Ok(changes);
        }

        [HttpGet("completed/{id}")]
        public async Task<ActionResult<ToDoItem>> GetItemStatus(long id)
        {
            var todoItem = await repositoryItems.GetItem(id);
            if (todoItem == null)
            {
                return StatusCode(400);
            }
            return Ok(todoItem.IsCompleted);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            var Res = await repositoryItems.GetItemsAsync();
            return Ok(Res);
        }

        [HttpPut]
        public async Task<IActionResult> PutTodoItem(long id, [FromBody]ToDoItem item)
        {
            if (id != item.ID)
            {
                return StatusCode(400);
            }
            await repositoryItems.UpdateItem(item);
            var response = new
            {
                completed = item.IsCompleted,
                recent_change = item.RecentUpdate
            };
            return Ok(response);
        }
    }
}

 