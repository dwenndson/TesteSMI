using System.Collections.Generic;
using System.Threading.Tasks;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.Services
{
    public interface IMeterKeyService
    {
        MeterKeys GetBySerial(string serial);
        KeysDto Create(KeysDto keysDto);
        KeysDto Update(KeysDto keysDto);
        List<KeysDto> GetAll();
        Task<IActionResult> ProcessFile(IFormFile file);

    }
}