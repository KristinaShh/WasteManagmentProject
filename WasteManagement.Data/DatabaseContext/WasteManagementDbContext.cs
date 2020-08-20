using Microsoft.EntityFrameworkCore;
using WasteManagement.Models.Interfaces;
using WasteManagement.Models.Models;

namespace WasteManagement.Data.DatabaseContext
{
    public class WasteManagementDbContext : DbContext, IWasteManagementDbContext
    {
        public DbSet<WasteContainerDtoModel> Containers { get; set; }

        public WasteManagementDbContext(DbContextOptions<WasteManagementDbContext> options)
      : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=waste.db");
    }
}