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
                    var readJob = result.Content.ReadAsAsync<IList<TodoItem>>();
                    readJob.Wait();
                    item = readJob.Result;

                }
                else
                {
                    item = Enumerable.Empty<TodoItem>();
                    ModelState.AddModelError(string.Empty, "Server Error!");
                    
                }

            
            }
            return View(item);
        }

        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Create(TodoItem item)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44331/api/TodoItems");
                var postjob = client.PostAsJsonAsync<TodoItem>("TodoItems", item);
                postjob.Wait();

                var postRestult = postjob.Result;
                if (postRestult.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Server Error!");

            return View(item);
        }

        public ActionResult Edit(int id)
        {
            
            TodoItem item = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44331/api/");
                var responseTask = client.GetAsync("TodoItems/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TodoItem>();
                    item = readTask.Result;
                }
            }

            return View(item);
        }


        [HttpPost]
        public ActionResult Edit(TodoItem item)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44331/api/TodoItems");
                var putTask = client.PutAsJsonAsync<TodoItem>("TodoItems", item);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                return View(item);
            }
        
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44331/api/");
                var deleteTask = client.DeleteAsync("TodoItems/" + id);

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }




        }
    }
