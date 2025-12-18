namespace Business.Abstract
{
    public interface IChatbotService
    {
        Task<string> AskHabitCoachAsync(string userMessage);
    }
}
