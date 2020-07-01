namespace IogServices.Enums
{
    public enum TicketStatus
    {
        Waiting = 0,
        Executing = 1,
        Finished = 2,
        PartiallyFinished = 3,
        Interrupted = 4,
        MiddlewareNotResponding = 5,
        FailedToStart = 6,
        FailedToContinue = 7
    }
}