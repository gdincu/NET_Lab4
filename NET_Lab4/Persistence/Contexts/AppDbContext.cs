using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
        public DbSet<Sarcina> Sarcini { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
