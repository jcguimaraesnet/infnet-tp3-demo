using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using profjulioguimaraes.Models;

namespace profjulioguimaraes.Data
{
    public class AmizadeDbContext : DbContext
    {
        public AmizadeDbContext (DbContextOptions<AmizadeDbContext> options)
            : base(options)
        {
        }

        public DbSet<profjulioguimaraes.Models.Amigo>? Amigo { get; set; }
    }
}
