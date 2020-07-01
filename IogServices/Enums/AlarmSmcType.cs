﻿﻿using System.ComponentModel;

  namespace IogServices.Enums
{
    public enum AlarmSmcType
    {
        [Description("Falha de Firmware da CPU")]
        CPU_FALHA_FIRMWARE = 0,
        [Description("Falha de Barramento da CPU")]
        CPU_FALHA_BARRAMENTO = 1,
        [Description("Falha de Flash da CPU")]
        CPU_FALHA_DFLASH = 2,
        [Description("Medidor Inválido da CPU")]
        CPU_MEDIDOR_INVALIDO = 3,
        [Description("Porta Aberta")]
        PORTA_ABERTA = 4,
        [Description("Corte Coletivo")]
        CORTE_COLETIVO = 5,
        [Description("Religamento Coletivo")]
        RELIGAMENTO_COLETIVO = 6,
        [Description("Escrita/Leitura Portátil")]
        ESCRITA_LEITURA_PORTATIL = 7,
        [Description("Modificações nas Configurações")]
        MODIF_CONFIG = 8,
        [Description("Modificações nas Configurações de Alarmes")]
        MODIF_CONFIG_ALARMS = 9,
        [Description("Modificações nas Configurações de Parâmetros")]
        MODIF_CONFIG_PARAMS = 10
    }
}
