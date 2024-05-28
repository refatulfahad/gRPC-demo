﻿using grpc_demo.Models;
using Microsoft.EntityFrameworkCore;

namespace grpc_demo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
