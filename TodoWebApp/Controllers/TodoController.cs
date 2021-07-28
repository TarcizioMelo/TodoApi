using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TodoWebApp.Models;

namespace TodoWebApp.Controllers
{
    public class TodoController : Controller
    {
        /*
        // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0        
        
        HttpClient is intended to be instantiated once and re-used throughout the life of an application.
        Instantiating an HttpClient class for every request will exhaust the number of sockets available under heavy loads.
        This will result in SocketException errors.Below is an example using HttpClient correctly.

        public class GoodController : ApiController
        {
            private static readonly HttpClient HttpClient;

            static GoodController()
            {
                HttpClient = new HttpClient();
            }
        }        
        */
        private static readonly HttpClient client;

        static TodoController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44331/api/");
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<TodoItem> item = null;

            var result = await client.GetAsync("TodoItems");
            if (result.IsSuccessStatusCode)
            {
                item = await result.Content.ReadAsAsync<IList<TodoItem>>();
            }
            else
            {
                item = Enumerable.Empty<TodoItem>();
                ModelState.AddModelError(string.Empty, "Server Error!");
            }

            return View(item);
        }

        public ActionResult Create() => View();

        [HttpPost]
        public async Task<ActionResult> Create(TodoItem item)
        {
            var postRestult = await client.PostAsJsonAsync<TodoItem>("TodoItems", item);

            if (postRestult.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Server Error!");

            return View(item);
        }
    }
}
