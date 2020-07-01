using System;
using System.Collections.Generic;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Util;
using MqttClientLibrary.Models;
using Command = NetworkObjects.Enum.Command;
using CommunicationStatus = NetworkObjects.Enum.CommunicationStatus;

namespace IogServices.Threads
{
    public interface ITicketThread : IDisposable
    {
        void Run();
        void AddTicket(TicketDto ticket, PriorityValue priorityValue, IEnumerable<ForwarderMessage> forwarderMessage);
        int GetNumberOfCommands();
        void SetExecutingCommand(bool value);
        bool CheckIfIsExecutingCommand();
        bool IsDying();
        void ReceivedUpdate(object sender, MessageReceivedArgs messageReceivedArgs);
        string GetDeviceSerial();
        Command GetCurrentCommand();
        CommunicationStatus GetCommunicationStatus();
    }
}