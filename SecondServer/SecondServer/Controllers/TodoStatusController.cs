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
using SecondServer.Models.JsonModel;
using System.Text.Json;

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
        { GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                Response.StatusCode = 401;
                await Response.WriteAsync("Пользователь не авторизован");
            }
            item.UserId = userinfo.Id;
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
            GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                return StatusCode(403);
            }
            bool completed = await repositoryItems.DeleteItem(id, userinfo.Id);
            if (!completed)
                return BadRequest();
            return StatusCode(200);
        }

        [HttpPost("new/{task}/{completed}")]
        public async Task<ActionResult<ToDoItem>> PostToDoItem( bool completed, int task) {
            GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                return StatusCode(403);
            }
            ToDoItem item = new ToDoItem(completed, task,userinfo.Id);
            bool done = await repositoryItems.AddItem(item);
            if (!done)
            {
                return StatusCode(500);
             }
            Dictionary<string, long> d = new Dictionary<string, long> { { "id", item.ID } };
            return Ok(d);
        }

    [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetTodoItem(long id)
        {
            GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                return StatusCode(403);
            }
            var todoItem = await repositoryItems.GetItem(id);
            if (todoItem == null || todoItem.UserId!=userinfo.Id)
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
            GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                return StatusCode(403);
            }
            var item = await repositoryItems.GetItem(id);
            if (item == null || item.UserId!= userinfo.Id)
            {
                return StatusCode(400);
            }
             var changes = await repositoryItems.GetHistoryAsync(id, userinfo.Id);
            return Ok(changes);
        }

        [HttpGet("completed/{id}")]
        public async Task<ActionResult<ToDoItem>> GetItemStatus(long id)
        {
            GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                return StatusCode(403);
            }
            var todoItem = await repositoryItems.GetItem(id);
            if (todoItem == null || todoItem.UserId!=userinfo.Id)
            {
                return StatusCode(400);
            }
            return Ok(todoItem.IsCompleted);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            GetLogin userinfo = await GetLoginUser();
            if (userinfo == null)
            {
                return StatusCode(403);
            }
            var Res = await repositoryItems.GetItemsAsync(userinfo.Id);
            return Ok(Res);
        }

        [HttpPut]
        public async Task<IActionResult> PutTodoItem(long id, [FromBody]ToDoItem item)
        { 
            GetLogin userinfo = await GetLoginUser();
            if (userinfo==null )
            {
                return StatusCode(403);
            }
            if (id != item.ID)
            {
                return StatusCode(400);
            }
            await repositoryItems.UpdateItem(item, userinfo.Id);
            var response = new
            {
                completed = item.IsCompleted,
                recent_change = item.RecentUpdate
            };
            return Ok(response);
        }

     public async Task<GetLogin> GetLoginUser()
        {
            var Authorization = Request.Headers["Authorization"].ToString();
            var request = new HttpRequestMessage(HttpMethod.Post,
                "http://localhost:4000/api/auth/getlogin");
            request.Headers.Add("Authorization", Authorization);

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();

            if (response.StatusCode.ToString() == "OK")
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                GetLogin userinfo = System.Text.Json.JsonSerializer.Deserialize<SecondServer.Models.JsonModel.GetLogin>(data, options);
                return userinfo;
            }
            return null;
        }
    }
}

 