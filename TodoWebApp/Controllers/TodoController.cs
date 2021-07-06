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
        public IActionResult Index()
        {
            IEnumerable<TodoItem> item = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44331/api/");
                var responseTask = client.GetAsync("TodoItems");
                responseTask.Wait();

                var result = responseTask.Result;
                Console.WriteLine(result);
                if (result.IsSuccessStatusCode)
                {
                    var ReadJob = result.Content.ReadAsAsync<IList<TodoItem>>();
                    ReadJob.Wait();
                    item = ReadJob.Result;

                }
                else
                {
                    item = Enumerable.Empty<TodoItem>();
                    ModelState.AddModelError(string.Empty, "Server Error");
                    
                }

            
            }
            return View(item);
        }
    }
}
