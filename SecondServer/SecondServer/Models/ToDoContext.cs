using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecondServer.Abstractions;
 
namespace SecondServer.Models
{
    public class ToDoContext : DbContext, IStorageContext //обеспечение доступа к данным БД 
    {
        public ToDoContext(DbContextOptions<ToDoContext> options):
            base(options)
        {  }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoChange> ToDoChange { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//indexing table to decrease number of works
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ToDoItem>().HasKey(x => x.ID);
            modelBuilder.Entity<ToDoItem>().ToTable("ToDoItems");
            modelBuilder.Entity<ToDoItem>().HasIndex(x => x.IsCompleted);
            modelBuilder.Entity<ToDoItem>().HasIndex(x => x.RecentUpdate);
        }
    }
}