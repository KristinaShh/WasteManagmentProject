using System.Collections.Generic;
using WasteManagement.Models.Models;

namespace WasteManagement.Models.Interfaces
{
    public interface IWasteContainerRepository : IRepository<WasteContainerDtoModel>
    {
        List<WasteContainerDtoModel> GetByWasteContainerName(string searchString);
    }
}
