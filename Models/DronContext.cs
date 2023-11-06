using Microsoft.EntityFrameworkCore;

namespace Drones.Models
{
    public class DronContext : DbContext
    {
        public DronContext(DbContextOptions<DronContext> options)
            : base(options)
        {
        }
        public DbSet<Dron> DronItems { get; set; } = null!;
    }
}
