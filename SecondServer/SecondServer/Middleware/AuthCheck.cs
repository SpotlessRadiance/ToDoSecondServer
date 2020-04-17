using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace SecondServer.Middleware
{
    public class AuthCheck
    {
        private readonly RequestDelegate _next;

        public AuthCheck(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IHttpClientFactory clientFactory)
        {
            var Authorization = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(Authorization))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token is invalid ");
            }
            else
            {
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
                    var jsonModel = JsonSerializer.Deserialize<SecondServer.Models.JsonModel.GetLogin>(data, options);
                    if (jsonModel.Authorized == true)
                    {
                        await _next.Invoke(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync(response.StatusCode.ToString());
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(response.StatusCode.ToString());
                }
            }
        }
    }
}
