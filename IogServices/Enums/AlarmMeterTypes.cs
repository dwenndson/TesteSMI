﻿﻿using System.ComponentModel;

 namespace EletraSmcModels
{
    public enum AlarmMeterTypes
    {
        [Description("Medidor Programa")]
        MEDIDOR_PROGRAMA = 0,
        [Description("Medidor Energia")]
        MEDIDOR_ENERGIA = 1,
        [Description("Falha de Parâmetros do Medidor")]
        MEDIDOR_FALHA_PARAMETROS = 2,
        [Description("Corrente Reversa")]
        MEDIDOR_CORRENTE_REVERSA = 3,
        [Description("Timeout")]
        MEDIDOR_TIMEOUT = 4,
        [Description("Corte no Medidor")]
        MEDIDOR_CORTE = 5,
        [Description("Religamento no Medidor")]
        MEDIDOR_RELIG = 6
    }
}
