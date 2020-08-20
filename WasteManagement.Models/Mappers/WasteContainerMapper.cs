using System.Collections.Generic;
using WasteManagement.Models.Models;

namespace WasteManagement.Models.Mappers
{
    public static class WasteContainerMapper
    {

        public static WasteContainerUpdateModel MapToWasteContainerEditModel(WasteContainerDtoModel entity)
        {
            var editModel = new WasteContainerUpdateModel();

            editModel.Id = entity.Id;
            editModel.Name = entity.Name;
            editModel.Location = entity.Location;
            editModel.FullPecentage = entity.FullPecentage;

            return editModel;
        }

        public static WasteContainerViewModel MapToWasteContainerViewModel(WasteContainerDtoModel entity)
        {
            return new WasteContainerViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                FullPecentage = entity.FullPecentage
            };
        }


        public static List<WasteContainerViewModel> MapToWasteContainerViewModel(List<WasteContainerDtoModel> entities)
        {
            List<WasteContainerViewModel> viewModelList = new List<WasteContainerViewModel>();

            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    viewModelList.Add(
                        new WasteContainerViewModel()
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            Location = entity.Location,
                            FullPecentage = entity.FullPecentage
                        }
                    );
                }
            }

            return viewModelList;
        }

    }
}