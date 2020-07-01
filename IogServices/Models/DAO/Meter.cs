using System.Collections.Generic;
using IogServices.Enums;
using IogServices.Models.DAO.Generic;
using IogServices.Models.DTO;
using NetworkObjects.Enum;

namespace IogServices.Models.DAO
{
    public class Meter : Base
    {
        public string Serial { get; set; }
        public string Installation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Prefix { get; set; }
        public decimal BillingConstant { get; set; }
        public string TcRatio { get; set; }
        public string TpRatio { get; set; }
        public string Registrars { get; set; }
        public string Tli { get; set; }
        public MeterIdentifier Identifier { get; set; }
        public MeterPhase Phase { get; set; }
        public MeterConnectionPhase ConnectionPhase { get; set; }
        public AccountantStatus AccountantStatus { get; set; }
        public MeterModel MeterModel { get; set; }
        public RateType RateType { get; set; }

        public Modem Modem { get; set; }
        public Smc Smc { get; set; }
        public MeterKeys MeterKeys { get; set; }
        public List<MeterEnergy> Energies { get; set; }
        public List<MeterAlarm> MeterAlarms { get; set; }
        public bool Comissioned { get; set; }

        public void UpdateFields(Meter meter, MeterModel model, RateType rateType, Smc smc, MeterKeys meterKeys)
        {
            Serial = meter.Serial;
            Installation = meter.Installation;
            Latitude = meter.Latitude;
            Longitude = meter.Longitude;
            Prefix = meter.Prefix;
            BillingConstant = meter.BillingConstant;
            TcRatio = meter.TcRatio;
            TpRatio = meter.TpRatio;
            Registrars = meter.Registrars;
            Tli = meter.Tli;
            Identifier = meter.Identifier;
            Phase = meter.Phase;
            ConnectionPhase = meter.ConnectionPhase;
            AccountantStatus = meter.AccountantStatus;
            MeterModel = model;
            RateType = rateType;
            Smc = smc;
            Energies = meter.Energies;
        }
        
        public void UpdateFields(Meter meter, MeterModel model, RateType rateType, MeterKeys meterKeys)
        {
            Serial = meter.Serial;
            Installation = meter.Installation;
            Latitude = meter.Latitude;
            Longitude = meter.Longitude;
            Prefix = meter.Prefix;
            BillingConstant = meter.BillingConstant;
            TcRatio = meter.TcRatio;
            TpRatio = meter.TpRatio;
            Registrars = meter.Registrars;
            Tli = meter.Tli;
            Identifier = meter.Identifier;
            Phase = meter.Phase;
            ConnectionPhase = meter.ConnectionPhase;
            AccountantStatus = meter.AccountantStatus;
            MeterModel = model;
            RateType = rateType;
            Smc = meter.Smc;
            Energies = meter.Energies;
            MeterKeys = meterKeys;
        }
    }
}