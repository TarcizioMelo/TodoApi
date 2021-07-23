using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<TodoItem>()
                .HasData(
                    new TodoItem { Id = 1, Name = "Walk Dog", IsComplete = true },
                    new TodoItem { Id = 2, Name = "Do The Dishes", IsComplete = true },
                    new TodoItem { Id = 3, Name = "Clean Room", IsComplete = true }
                );

        }




    }






}
