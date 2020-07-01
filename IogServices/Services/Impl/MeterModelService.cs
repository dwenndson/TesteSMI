using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using Microsoft.EntityFrameworkCore.Internal;

namespace IogServices.Services.Impl
{
    public class MeterModelService : IMeterModelService
    {
        private readonly IMeterModelRepository _meterModelRepository;
        private readonly IManufacturerService _manufacturerService;
        private readonly IMapper _mapper;

        public MeterModelService(IMeterModelRepository meterModelRepository, IManufacturerService manufacturerService,
            IMapper mapper)
        {
            _meterModelRepository = meterModelRepository;
            _manufacturerService = manufacturerService;
            _mapper = mapper;
        }

        public List<MeterModelDto> GetAll()
        {
            return _mapper.Map<List<MeterModel>, List<MeterModelDto>>(_meterModelRepository.GetAll());
        }

        public List<MeterModelDto> GetByManufacturer(string manufacturerName)
        {
            var manufacturer = _manufacturerService.GetExistingManufacturer(manufacturerName);
            return _mapper.Map<List<MeterModel>, List<MeterModelDto>>(manufacturer.MeterModels
                .Where(meterModel => meterModel.Active).OrderByDescending(meterModel => meterModel.UpdatedAt).ToList());
        }

        public MeterModelDto GetByName(string name)
        {
            return _mapper.Map<MeterModel, MeterModelDto>(_meterModelRepository.GetActiveByName(name));
        }

        public MeterModelDto Save(MeterModelDto meterModelDto)
        {
            meterModelDto.Name = meterModelDto.Name.ToUpper();
            var savedMeterModel = _meterModelRepository.GetActiveByName(meterModelDto.Name);
            var meterModel = _mapper.Map<MeterModelDto, MeterModel>(meterModelDto);
            meterModel.Manufacturer = _manufacturerService.GetExistingManufacturer(meterModelDto.Manufacturer.Name);
            if (savedMeterModel == null)
                return _mapper.Map<MeterModel, MeterModelDto>(_meterModelRepository.Save(meterModel));
            
            if (savedMeterModel.Active)
                throw new ExistentEntityException("O modelo de smc de nome " + savedMeterModel.Name + " já existe");
            
            savedMeterModel.UpdateFields(meterModel);
            savedMeterModel.Active = true;
            return _mapper.Map<MeterModel, MeterModelDto>(
                 _meterModelRepository.Update(savedMeterModel));

        }

        public MeterModelDto Update(MeterModelDto meterModelDto)
        {
            meterModelDto.Name = meterModelDto.Name.ToUpper();
            var savedMeterModel = GetExistingMeterModel(meterModelDto.Name);
            var meterModel = _mapper.Map<MeterModelDto, MeterModel>(meterModelDto);
            var manufacturer =
                _manufacturerService.GetExistingManufacturer(meterModelDto.Manufacturer.Name);
            meterModel.Manufacturer = manufacturer;
            savedMeterModel.UpdateFields(_mapper.Map<MeterModelDto, MeterModel>(meterModelDto));
            return _mapper
                .Map<MeterModel, MeterModelDto>(
                    _meterModelRepository.Update(savedMeterModel)
                );
        }

        public void Deactivate(string name)
        {
            var meterModel = GetExistingMeterModel(name);
            if (meterModel.Meters.Any())
                throw new NotNullForeignKeyReferenceException(
                    "O modelo de medidor informado possui relação com outras entidades e por isso não pode ser excluído");
            meterModel.Active = false;
            _meterModelRepository.Update(meterModel);
        }

        public MeterModel GetExistingMeterModel(string name)
        {
            var savedMeterModel = _meterModelRepository.GetActiveByName(name.ToUpper());
            if (savedMeterModel == null)
                throw new InvalidConstraintException("O modelo de medidor informado é inválido");
            return savedMeterModel;
        }
    }
}