using System;
using IogServices.Enums;
using IogServices.Models.DAO;
using Microsoft.AspNetCore.Http;

namespace IogServices.Util
{
    public interface IServicesUtils
    {
        Uri CreateUri(HttpRequest request, string routeUrl);
        ClientType GetCurrentClientType(string token);
        void RemoveModemOfAllEntities(Modem modem);
    }
}