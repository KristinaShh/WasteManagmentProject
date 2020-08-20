using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using WasteManagement.Models.Interfaces;
using WasteManagement.Models.Mappers;
using WasteManagement.Models.Models;

namespace WasteContainer.Services
{
    public class WasteContainerService : IWasteContainerService
    {
        private readonly IWasteContainerRepository _repository;
        private readonly ILogger _logger;

        private readonly int CultureId = Thread.CurrentThread.CurrentCulture.LCID;
        private string LOCALIZATION_ENTITY_NOT_FOUND => "Localization_WasteContainer_Not_Found";
        private string LOCALIZATION_GENERAL_NOT_FOUND => "Localization_General_Not_Found";

        public WasteContainerService(IWasteContainerRepository repository, ILogger<WasteContainerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public bool Add(WasteContainerCreateModel entity)
        {
            WasteContainerDtoModel wasteContainer = new WasteContainerDtoModel
            {
                Name = entity.Name,
                FullPecentage = entity.FullPecentage,
                Location = entity.Location,
                Size = entity.Size
            };


            return _repository.Add(wasteContainer);
        }

        public bool Delete(Guid id)
        {
            try
            {
                return _repository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while deleting waste containers : {0} - {1} ", ex.StackTrace, ex.Message);
                throw ex;
            }
        }


        public bool Exists(Guid id)
        {
            return _repository.Exists(id);
        }

        public List<WasteContainerViewModel> GetWasteContainers()
        {
            var entities = _repository.GetAll();
            if (entities == null)
            {
                throw new Exception(LOCALIZATION_ENTITY_NOT_FOUND);
            }

            return WasteContainerMapper.MapToWasteContainerViewModel(entities);
        }

        public List<WasteContainerViewModel> GetWasteContainers(string columnName, string searchString)
        {
            List<WasteContainerDtoModel> entities;

            switch (columnName.ToLower())
            {
                case "name":
                    entities = _repository.GetByWasteContainerName(searchString);
                    break;
                default:
                    entities = _repository.GetAll();
                    break;
            }

            if (entities == null)
            {
                throw new Exception(LOCALIZATION_ENTITY_NOT_FOUND);
            }

            return WasteContainerMapper.MapToWasteContainerViewModel(entities);
        }

        public WasteContainerUpdateModel GetWasteContainerAsEditModel(Guid id)
        {
            var entity = _repository.Get(id);
            if (entity == null)
            {
                throw new Exception(LOCALIZATION_ENTITY_NOT_FOUND);
            }

            return WasteContainerMapper.MapToWasteContainerEditModel(entity);
        }

        public WasteContainerViewModel GetWasteContainerAsViewModel(Guid id)
        {
            var entity = _repository.Get(id);
            if (entity == null)
            {
                throw new Exception(LOCALIZATION_ENTITY_NOT_FOUND);
            }

            return WasteContainerMapper.MapToWasteContainerViewModel(entity);
        }

        public bool Update(WasteContainerUpdateModel wasteContainer)
        {
            WasteContainerDtoModel entity = _repository.Get(wasteContainer.Id);
            if (entity == null)
            {
                throw new Exception(LOCALIZATION_GENERAL_NOT_FOUND + entity.Id);
            }

            entity.Name = wasteContainer.Name;
            entity.FullPecentage = wasteContainer.FullPecentage;
            entity.Location = wasteContainer.Location;
            entity.Size = wasteContainer.Size;

            //entity.ModifiedAt = wasteContainer.ModifiedAt;
            //entity.ModifiedBy = wasteContainer.ModifiedBy;

            return _repository.Update(entity);
        }


    }
}