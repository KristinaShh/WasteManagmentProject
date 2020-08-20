using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteManagement.Models.Interfaces;
using WasteManagement.Models.Models;

namespace WasteManagement.Data.Repositories
{
    public class WasteContainerRepository : IWasteContainerRepository
    {
        private readonly IWasteManagementDbContext _context;
        public WasteContainerRepository(IWasteManagementDbContext context)
        {
            _context = context;
        }

        public bool Add(WasteContainerDtoModel entity)
        {
            _context.Add(entity);
            return SaveChangesResult();
        }

        public bool Delete(Guid id)
        {
            try
            {
                var entity = _context.Containers.FirstOrDefaultAsync(m => m.Id == id);
                var result = entity.Result;
                if (result == null)
                {
                    throw new Exception();
                }

                _context.Containers.Remove(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SaveChangesResult();
        }

        public bool Exists(Guid id)
        {
            return _context.Containers.Any(e => e.Id == id);
        }

        public WasteContainerDtoModel Get(Guid id)
        {
            var entity = _context.Containers.FirstOrDefaultAsync(m => m.Id == id);
            var result = entity.Result;
            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }

        public List<WasteContainerDtoModel> GetAll()
        {
            var cities = _context.Containers.ToListAsync();

            return cities.Result;
        }

        public List<WasteContainerDtoModel> GetByWasteContainerName(string searchString)
        {
            var entities = _context.Containers.Where(container => container.Name.Contains(searchString)).ToListAsync();

            return entities.Result;
        }


        public bool Update(WasteContainerDtoModel entity)
        {
            _context.Update(entity);
            return SaveChangesResult();
        }

        public bool SaveChangesResult()
        {
            try
            {
                var result = _context.SaveChangesAsync();
                if (result.Result == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }
}
