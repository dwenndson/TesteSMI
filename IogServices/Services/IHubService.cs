using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IHubService
    {
        Task GeneralSendUpdate(string method, object obj);
    }
}
