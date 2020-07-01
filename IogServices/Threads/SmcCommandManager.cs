using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DTO;

namespace IogServices.Threads
{
    public class SmcCommandManager
    {
        private readonly List<CommandThread> _commandThreads;
        public SmcDto SmcDto { get; }

        public SmcCommandManager(SmcDto smcDto)
        {
            _commandThreads = new List<CommandThread>();
            SmcDto = smcDto;
        }

        public void AddCommandThreadIfNotExist(CommandThread commandThread)
        {
            if(_commandThreads.Contains(commandThread)) return;
            _commandThreads.Add(commandThread);
        }

        public int GetSumOfCommands()
        {
            var numberOfCommands = _commandThreads.Sum(thread => thread.GetNumberOfCommands());

            return numberOfCommands;
        }
    }
}