using Microsoft.EntityFrameworkCore;

namespace Drones.Models
{
    public class MedicineContext : DbContext
    {
        public MedicineContext(DbContextOptions<MedicineContext> options)
            : base(options)
        {
        }
        public DbSet<Medicine> MedicineItems { get; set; } = null!;
    }
}
