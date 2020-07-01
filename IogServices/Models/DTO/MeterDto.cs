using System.ComponentModel.DataAnnotations;
using IogServices.Enums;
using IogServices.Models.DTO.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetworkObjects.Enum;
using CommunicationStatus = NetworkObjects.Enum.CommunicationStatus;

namespace IogServices.Models.DTO
{
    public class MeterDto : BaseDto
    {
        [Required]
        public string Serial { get; set; }
        [Required]
        public string Installation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Prefix { get; set; }
        public decimal BillingConstant { get; set; }
        public string TcRatio { get; set; }
        public string TpRatio { get; set; }
        public string Registrars { get; set; }
        public string Tli { get; set; }
        [Required]
        public MeterIdentifier Identifier { get; set; }
        [Required]
        public MeterPhase Phase { get; set; }
        public MeterConnectionPhase ConnectionPhase { get; set; }
        public AccountantStatus AccountantStatus { get; set; }
        [Required]
        public MeterModelDto MeterModel { get; set; }
        [Required]
        public RateTypeDto RateType { get; set; }
        public SmcDto Smc { get; set; }
        public ModemDto Modem { get; set; }
        public KeysDto KeysDto { get; set; }
        public CommunicationStatus CommunicationStatus { get; set; }
        public Command Command { get; set; }
        public int CommandQueue { get; set; }
        public bool Comissioned { get; set; }

    }
}