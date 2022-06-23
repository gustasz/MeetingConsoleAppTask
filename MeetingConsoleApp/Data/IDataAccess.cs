using MeetingConsoleApp.Models;

namespace MeetingConsoleApp.Data
{
    public interface IDataAccess
    {
        void AddPersonToMeeting(Person person, int meetingId);
        List<Meeting> GetMeetings();
        Person GetOrCreatePerson(string name, int id = 0);
        List<Person> GetPeople();
        Meeting? GetSingleMeeting(int id);
        bool MeetingExists(int id);
        bool PersonExists(int id);
        void RemoveMeeting(int id, int userId);
        void RemovePersonFromMeeting(Person person, int meetingId);
        void WriteMeeting(Meeting meeting);
    }
}