using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;

namespace IogServices.Services.Impl
{
    public class SmcModelService : ISmcModelService
    {
        private readonly ISmcModelRepository _smcModelRepository;
        private readonly IManufacturerService _manufacturerService;
        private readonly IMapper _mapper;

        public SmcModelService(ISmcModelRepository smcModelRepository, IManufacturerService manufacturerService,
            IMapper mapper)
        {
            _smcModelRepository = smcModelRepository;
            _manufacturerService = manufacturerService;
            _mapper = mapper;
        }

        public List<SmcModelDto> GetAll()
        {
            return _mapper.Map<List<SmcModel>, List<SmcModelDto>>(_smcModelRepository.GetAll());
        }

        public List<SmcModelDto> GetByManufacturer(string manufacturerName)
        {
            var manufacturer = _manufacturerService.GetExistingManufacturer(manufacturerName);
            return _mapper.Map<List<SmcModel>, List<SmcModelDto>>(manufacturer.SmcModels
                .Where(smcModel => smcModel.Active).OrderByDescending(smcModel => smcModel.UpdatedAt).ToList());
        }

        public SmcModelDto GetByName(string name)
        {
            return _mapper.Map<SmcModel, SmcModelDto>(_smcModelRepository.GetByName(name));
        }

        public SmcModelDto Save(SmcModelDto smcModelDto)
        {
            smcModelDto.Name = smcModelDto.Name.ToUpper();
            var savedSmcModel = _smcModelRepository.GetByName(smcModelDto.Name);
            var smcModel = _mapper.Map<SmcModelDto, SmcModel>(smcModelDto);
            
            smcModel.Manufacturer = _manufacturerService.GetExistingManufacturer(smcModelDto.Manufacturer.Name);
            
            if (savedSmcModel == null)
                return _mapper.Map<SmcModel, SmcModelDto>(_smcModelRepository.Save(smcModel));
            
            if (savedSmcModel.Active)
                throw new ExistentEntityException("O tipo de SMC de nome " + savedSmcModel.Name + " já existe");

            savedSmcModel.UpdateFields(smcModel);
            savedSmcModel.Active = true;
            return _mapper.Map<SmcModel, SmcModelDto>(
                 _smcModelRepository.Update(savedSmcModel));

        }

        public SmcModelDto Update(SmcModelDto smcModelDto)
        {
            smcModelDto.Name = smcModelDto.Name.ToUpper();
            var savedSmcModel = GetExistingSmcModel(smcModelDto.Name);
            var smcModel = _mapper.Map<SmcModelDto, SmcModel>(smcModelDto);
           
            var manufacturer = _manufacturerService.GetExistingManufacturer(smcModelDto.Manufacturer.Name);
            smcModel.Manufacturer = manufacturer;
            
            savedSmcModel.UpdateFields(smcModel);
            return _mapper
                .Map<SmcModel, SmcModelDto>(
                    _smcModelRepository.Update(savedSmcModel)
                );
        }

        public void Deactivate(string name)
        {
            var smcModel = GetExistingSmcModel(name);

            if (smcModel.Smcs.Any())
                throw new NotNullForeignKeyReferenceException(
                    "O modelo de smc informado possui relação com outras entidades e por isso não pode ser excluído");
            smcModel.Active = false;
            _smcModelRepository.Update(smcModel);
        }

        public SmcModel GetExistingSmcModel(string name)
        {
            var savedSmcModel = _smcModelRepository.GetByName(name.ToUpper());
            if (savedSmcModel == null)
                throw new InvalidConstraintException("O modelo de SMC informado é inválido");
            return savedSmcModel;
        }
    }
}