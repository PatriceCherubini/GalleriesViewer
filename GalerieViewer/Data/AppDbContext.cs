using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalerieViewer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GalerieViewer.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Galerie> Galeries { get; set; }
        public DbSet<ImageItem> ImageItems { get; set; }
    }
}
