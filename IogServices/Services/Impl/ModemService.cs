using System.Collections.Generic;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;

namespace IogServices.Services.Impl
{
    public class ModemService : IModemService
    {
        private readonly IModemRepository _modemRepository;
        private readonly IMapper _mapper;

        public ModemService(IModemRepository modemRepository, IMapper mapper)
        {
            _modemRepository = modemRepository;
            _mapper = mapper;
        }
        
        public List<ModemDto> GetAll()
        {
            return _mapper.Map<List<Modem>, List<ModemDto>>(_modemRepository.GetAll());
        }

        public ModemDto Save(ModemDto modemDto)
        {
            var modem = _mapper.Map<ModemDto, Modem>(modemDto);
            return _mapper.Map<Modem, ModemDto>(_modemRepository.Save(modem));
        }

        public ModemDto Update(ModemDto modemDto)
        {
            var modem = _mapper.Map<ModemDto, Modem>(modemDto);
            var savedModem = GetExistingModem(modem.DeviceEui);
            savedModem.UpdateFields(modem);
            return _mapper.Map<Modem, ModemDto>(_modemRepository.Update(savedModem));
        }

        public Modem GetExistingModem(string eui)
        {
            var savedModem = _modemRepository.GetByEui(eui);
            if (savedModem == null)
                throw new InvalidConstraintException("O modem informado é inválido");
            
            return savedModem;
        }
        
    }
}