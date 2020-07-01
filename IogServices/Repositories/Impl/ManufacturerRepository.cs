using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly IogContext _iogContext;

        public ManufacturerRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }

        public List<Manufacturer> GetAll()
        {
           return _iogContext.Manufacturers.Where(
                        manufacturer => manufacturer.Active)
                    .OrderByDescending(manufacturer => manufacturer.UpdatedAt).ToList();
        }

        public Manufacturer GetActiveByName(string name)
        {
            return _iogContext.Manufacturers
                .Include(
                    manufacturerIterator => manufacturerIterator.MeterModels)
                .Include(manufacturerIterator => manufacturerIterator.SmcModels)
                .FirstOrDefault(manufacturerIterator =>
                    manufacturerIterator.Name.Equals(name)  && manufacturerIterator.Active);
        }

        public Manufacturer GetByName(string name)
        {
            return _iogContext.Manufacturers
                .Include(
                    manufacturerIterator => manufacturerIterator.MeterModels)
                .Include(manufacturerIterator => manufacturerIterator.SmcModels)
                .FirstOrDefault(manufacturerIterator =>
                    manufacturerIterator.Name == name);        
        }

        public Manufacturer GetById(Guid id)
        {
            return _iogContext.Manufacturers
                .Include(manufacturerIterator => manufacturerIterator.MeterModels)
                .Include(manufacturerIterator => manufacturerIterator.SmcModels)
                .FirstOrDefault(manufacturerIterator =>
                    manufacturerIterator.Id == id && manufacturerIterator.Active);
        }

        public Manufacturer Save(Manufacturer manufacturer)
        {
            _iogContext.Manufacturers.Add(manufacturer);
            _iogContext.SaveChanges();
            return manufacturer;
        }

        public Manufacturer Update(Manufacturer manufacturer)
        {
            _iogContext.Update(manufacturer);
            _iogContext.SaveChanges();
            return manufacturer;
        }
    }
}