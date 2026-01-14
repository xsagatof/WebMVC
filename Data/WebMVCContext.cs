using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebMVC.Models;

namespace WebMVC.Data
{
    public class WebMVCContext : IdentityDbContext<User>
    {
        public WebMVCContext (DbContextOptions<WebMVCContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;

    }
}
