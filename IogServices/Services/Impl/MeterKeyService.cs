using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IogServices.Services.Impl
{
    public class MeterKeyService : IMeterKeyService
    {
        private readonly IMeterKeyRepository _meterKeyRepository;
        private readonly IMeterRepository _meterRepository;
        private readonly ISmcForwarderService _smcForwarderService;
        private readonly IMeterForwarderService _meterForwarderService;

        private readonly IMapper _mapper;

        public MeterKeyService(IMeterKeyRepository meterKeyRepository,  IMapper mapper, IMeterRepository meterRepository, ISmcForwarderService smcForwarderService, IMeterForwarderService meterForwarderService)
        {
            _meterKeyRepository = meterKeyRepository;
            _meterRepository = meterRepository;
            _smcForwarderService = smcForwarderService;
            _meterForwarderService = meterForwarderService;
            _mapper = mapper;
        }
        public MeterKeys GetBySerial(string serial)
        {
            var meterKeys = _meterKeyRepository.GetBySerial(serial);
            if (meterKeys != null) return meterKeys;
            
            meterKeys = new MeterKeys
            {
                Ak = "00000000000000000000000000000000",
                Ek = "00000000000000000000000000000000",
                Mk = "000102030405060708090A0B0C0D0E0F",
                Serial = serial
            };
            
            return _meterKeyRepository.Create(meterKeys);
        }

        public KeysDto Create(KeysDto keysDto)
        {
            var meterKeys = _meterKeyRepository.GetBySerial(keysDto.Serial);
            if (meterKeys != null)   
                throw new ExistentEntityException("As chaves do medidor de serial " + keysDto.Serial + " já existem na base de dados");
            return _mapper.Map<MeterKeys,KeysDto>(_meterKeyRepository.Create(_mapper.Map<KeysDto, MeterKeys>(keysDto)));
        }

        public KeysDto Update(KeysDto keysDto)
        {
            var meterKeys = GetExistingMeterKey(keysDto.Serial);
            meterKeys.UpdateFields(keysDto);
            var meter = _meterRepository.GetBySerial(keysDto.Serial);
            if (meter == null)
                return _mapper.Map<MeterKeys, KeysDto>(_meterKeyRepository.Update(meterKeys));
            HttpResponseMessage status;
            meter.MeterKeys = meterKeys;
            if (meter.Smc == null)
                status = _meterForwarderService.ForwardEdition(_mapper.Map<Meter, MeterDto>(meter));
            else
            {
                var smc = _mapper.Map<SmcDto>(meter.Smc);
                smc.KeysDto = keysDto;
                status = _smcForwarderService.ForwardEdition(smc);
            }    
            
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
                
            return _mapper.Map<MeterKeys, KeysDto>(_meterKeyRepository.Update(meterKeys));
            
        }

        public List<KeysDto> GetAll()
        {
            return _mapper.Map<List<MeterKeys>, List<KeysDto>>(_meterKeyRepository.GetAll());
        }

        public Task<IActionResult> ProcessFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public MeterKeys GetExistingMeterKey(string serial)
        {
            var savedKey = _meterKeyRepository.GetBySerial(serial);
            if (savedKey == null)
                throw new InvalidConstraintException("O medidor informado é inválido");
            return savedKey;
        }
    }
}