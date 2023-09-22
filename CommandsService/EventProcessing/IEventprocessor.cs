namespace CommandsService.EventProcessing
{
    public interface IEventprocessor
    {
        void ProcessEvent(string message);
    }
}