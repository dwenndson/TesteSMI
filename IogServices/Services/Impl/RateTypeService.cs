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
    public class RateTypeService : IRateTypeService
    {
        private readonly IRateTypeRepository _rateTypeRepository;
        private readonly IMapper _mapper;

        public RateTypeService(IRateTypeRepository rateTypeRepository, IMapper mapper)
        {
            _rateTypeRepository = rateTypeRepository;
            _mapper = mapper;
        }

        public List<RateTypeDto> GetAll()
        {
            return _mapper.Map<List<RateType>, List<RateTypeDto>>(_rateTypeRepository.GetAll());
        }

        public RateTypeDto GetByName(string name)
        {
            return _mapper.Map<RateType, RateTypeDto>(_rateTypeRepository.GetByName(name));
        }

        public RateTypeDto Save(RateTypeDto rateTypeDto)
        {
            rateTypeDto.Name = rateTypeDto.Name.ToUpper();
            var savedRateType = _rateTypeRepository.GetByName(rateTypeDto.Name);
            var rateType = _mapper.Map<RateTypeDto, RateType>(rateTypeDto);
            if (savedRateType == null)
                return _mapper.Map<RateType, RateTypeDto>(_rateTypeRepository.Save(rateType));
            
            if (savedRateType.Active)
                throw new ExistentEntityException("O tipo de tarifa de nome " + savedRateType.Name + " já existe");

            savedRateType.UpdateFields(rateType);
            savedRateType.Active = true;
            return _mapper.Map<RateType, RateTypeDto>(
                 _rateTypeRepository.Update(savedRateType));

        }

        public RateTypeDto Update(RateTypeDto rateTypeDto)
        {
            rateTypeDto.Name = rateTypeDto.Name.ToUpper();
            var savedRateType = GetExistingRateType(rateTypeDto.Name);
            savedRateType.UpdateFields(_mapper.Map<RateTypeDto, RateType>(rateTypeDto));
            return _mapper
                .Map<RateType, RateTypeDto>(
                    _rateTypeRepository.Update(savedRateType)
                );
        }

        public void Deactivate(string name)
        {
            var rateType = GetExistingRateType(name);
            if (rateType.Meters.Any())
                throw new NotNullForeignKeyReferenceException(
                    "O tipo de tarifa informado possui relação com outras entidades e por isso não pode ser excluído");
            rateType.Active = false;
            _rateTypeRepository.Update(rateType);
        }

        public RateType GetExistingRateType(string name)
        {
            var savedRateType = _rateTypeRepository.GetByName(name.ToUpper());
            if (savedRateType == null)
                throw new InvalidConstraintException("O tipo de tarifa informado é inválido");
            return savedRateType;
        }
    }
}