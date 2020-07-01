using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using InvalidConstraintException = IogServices.ExceptionHandlers.Exceptions.InvalidConstraintException;

namespace IogServices.Services.Impl
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _repository;
        private readonly IMapper _mapper;

        public ManufacturerService(IManufacturerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ManufacturerDto> GetAll()
        {
            return _mapper.Map<List<Manufacturer>, List<ManufacturerDto>>(_repository.GetAll());
        }

        public ManufacturerDto GetByName(string name)
        {
            return _mapper.Map<Manufacturer, ManufacturerDto>(_repository.GetActiveByName(name.ToUpper()));
        }

        public ManufacturerDto Save(ManufacturerDto manufacturerDto)
        {
            manufacturerDto.Name = manufacturerDto.Name.ToUpper();
            var savedManufacturer = _repository.GetActiveByName(manufacturerDto.Name);
            var manufacturer = _mapper.Map<ManufacturerDto, Manufacturer>(manufacturerDto);
            
            if (savedManufacturer == null)
                return _mapper.Map<Manufacturer, ManufacturerDto>(_repository.Save(manufacturer));
            
            if (savedManufacturer.Active)
                throw new ExistentEntityException("A marca de nome " + savedManufacturer.Name + " já existe");
            
            savedManufacturer.UpdateFields(manufacturer);
            savedManufacturer.Active = true;
            return _mapper.Map<Manufacturer, ManufacturerDto>(
                _repository.Update(savedManufacturer));
        }

        public ManufacturerDto Update(ManufacturerDto manufacturerDto)
        {
            manufacturerDto.Name = manufacturerDto.Name.ToUpper();
            var savedManufacturer = GetExistingManufacturer(manufacturerDto.Name);
            savedManufacturer.UpdateFields(_mapper.Map<ManufacturerDto, Manufacturer>(manufacturerDto));
            return _mapper
                .Map<Manufacturer, ManufacturerDto>(
                    _repository.Update(savedManufacturer)
                );
        }

        public void Deactivate(string name)
        {
            var manufacturer = GetExistingManufacturer(name);
            if (manufacturer.MeterModels.Any() || manufacturer.SmcModels.Any())
                throw new NotNullForeignKeyReferenceException(
                    "A marca informada possui relação com outras entidades e por isso não pode ser excluída");
            

            manufacturer.Active = false;
            _repository.Update(manufacturer);
        }

        public Manufacturer GetExistingManufacturer(string name)
        {
            var savedManufacturer = _repository.GetActiveByName(name.ToUpper());
            if (savedManufacturer == null)
                throw new InvalidConstraintException("A marca informada é inválida");
            return savedManufacturer;
        }
    }
}