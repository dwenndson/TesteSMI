using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Policy;
using IogServices.Enums;
using IogServices.Models.DAO;
using IogServices.Repositories;
using Microsoft.AspNetCore.Http;

namespace IogServices.Util.Impl
{
    public class ServicesUtils : IServicesUtils
    {
        private readonly IMeterRepository _meterRepository;
        private readonly ISmcRepository _smcRepository;

        public ServicesUtils(IMeterRepository meterRepository, ISmcRepository smcRepository)
        {
            _meterRepository = meterRepository;
            _smcRepository = smcRepository;
        }

        public Uri CreateUri(HttpRequest request, string routeUrl)
        {
            return new
                Uri(
                    $"{request.Scheme}://{request.Host}{routeUrl}",
                    UriKind.Absolute
                );
        }

        public ClientType GetCurrentClientType(string token)
        {
            ClientType clientType = ClientType.ALL;
            token = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var decodedToken = handler.ReadToken(token) as JwtSecurityToken;
            if (decodedToken != null)
            {
                var clientTypeArgument = decodedToken.Claims.First(claim => claim.Type == "clientType");
                if (clientTypeArgument != null)
                {
                    clientType = (ClientType) Enum.Parse(typeof(ClientType), clientTypeArgument.Value);   
                }
            }

            return clientType;
        }

        public void RemoveModemOfAllEntities(Modem modem)
        {
            List<Smc> smcs = _smcRepository.GetByModem(modem);
            List<Meter> meters = _meterRepository.GetByModem(modem);

            smcs.ForEach(smc =>
            {
                smc.Modem = null;
                _smcRepository.Update(smc);
            });

            meters.ForEach(meter =>
            {
                meter.Modem = null;
                _meterRepository.Update(meter);
            });
        }
        
        
    }
}