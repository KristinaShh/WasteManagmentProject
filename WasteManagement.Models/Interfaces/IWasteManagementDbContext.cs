using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
using WasteManagement.Models.Models;

namespace WasteManagement.Models.Interfaces
{
    public interface IWasteManagementDbContext
    {
        DbSet<WasteContainerDtoModel> Containers { get; set; }
        EntityEntry Add(object entity);
        EntityEntry Update(object entity);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
