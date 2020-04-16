using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecondServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondServer.Abstractions;
namespace SecondServer.Controllers
{
    [Produces("application/json")]
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private IItemsRepository repositoryItems;
        private IChangesRepository repositoryChanges;
        public TodoController(IItemsRepository repo, IChangesRepository repoCh)
        {
            repositoryItems = repo;
            repositoryChanges = repoCh;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostTodoItem(ToDoItem item)
        {
            bool completed = await repositoryItems.AddItem(item);
            if (!completed)
            {
                return StatusCode(500);
            }
            
            return CreatedAtAction(nameof(repositoryItems.GetItem), new { id = item.ID }, item);
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
            return todoItem;
        }

        [HttpGet("history/{id}")]
        public async Task<ActionResult> GetHistoryResult(long id)
        {
            var item = await repositoryItems.GetItem(id);
            if (item == null)
            {
                return StatusCode(400);
            }
             var changes = await repositoryItems.GetHistoryAsync();
            return Ok(changes);
        }

        [HttpGet("status/{id}")]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, [FromBody]ToDoItem item)
        {
            if (id != item.ID)
            {
                return StatusCode(400);
            }
            await repositoryItems.UpdateItem(item);
            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> PutTodoItem([FromBody]ToDoItem item)
        {
            bool completed = await repositoryItems.UpdateItem(item);
            if (!completed)
            {
                return StatusCode(200);
            }
            return Ok(item);
        }
    }
}

 