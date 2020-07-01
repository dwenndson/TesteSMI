namespace IogServices.Enums
{
    public enum CommandStatus
    {
        NoCommissioned = 0,
        WaitingCommand = 1,
        RegisteringKeys = 2,
        RegisteringDatetime = 3,
        RequestingDate = 4,
        RequestingRelayStatus = 5,
        ChangingRelayStatus = 6,
        RequestingSerial = 7
    }
}