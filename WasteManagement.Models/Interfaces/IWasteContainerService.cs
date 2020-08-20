using System;
using System.Collections.Generic;
using WasteManagement.Models.Models;

namespace WasteManagement.Models.Interfaces
{
    public interface IWasteContainerService
    {
        bool Exists(Guid id);
        bool Delete(Guid id);
        bool Add(WasteContainerCreateModel wasteContainer);
        bool Update(WasteContainerUpdateModel wasteContainer);
        List<WasteContainerViewModel> GetWasteContainers();
        List<WasteContainerViewModel> GetWasteContainers(string columnName, string searchString);
        WasteContainerViewModel GetWasteContainerAsViewModel(Guid cityId);
        WasteContainerUpdateModel GetWasteContainerAsEditModel(Guid cityId);
    }
}
