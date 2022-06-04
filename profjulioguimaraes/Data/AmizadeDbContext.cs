using Microsoft.EntityFrameworkCore;

namespace profjulioguimaraes.Data
{
    public class AmizadeDbContext : DbContext
    {
        public AmizadeDbContext (DbContextOptions<AmizadeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Amizade.Domain.Model.Entities.Amigo>? Amigo { get; set; }
    }
}
