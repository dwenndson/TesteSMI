using System;

namespace IogServices.Util
{
    public class SendCommandAnswerArgs : EventArgs
    {
        public string CommandAnswer { get; }

        public SendCommandAnswerArgs(string commandAnswer)
        {
            CommandAnswer = commandAnswer;
        }
    }
}