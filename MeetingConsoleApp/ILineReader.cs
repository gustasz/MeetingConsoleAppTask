
namespace MeetingConsoleApp
{
    public interface ILineReader
    {
        int GetAttendeeFilterType();
        int GetCategoryFromLine();
        int GetDateFilterType();
        DateTime GetDateFromLine(string question);
        int GetFilterIdFromLine();
        int GetIntFromLine(string question);
        string GetStringFromLine(string question);
        TimeOnly GetTimeFromLine(string question, TimeOnly start);
        int GetTypeFromLine();
    }
}