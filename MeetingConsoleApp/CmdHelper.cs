using MeetingConsoleApp.Data;
using MeetingConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingConsoleApp
{
    public class CmdHelper
    {
        readonly IDataAccess _dataAccess;
        readonly ILineReader _lineReader;
        public CmdHelper(IDataAccess dataAccess, ILineReader lineReader)
        {
            _dataAccess = dataAccess;
            _lineReader = lineReader;
        }
        public void CreateMeeting()
        {
            Meeting meeting = new();

            meeting.Name = _lineReader.GetStringFromLine("Name of the meeting:");
            var personInput = _lineReader.GetStringFromLine("Name or id of the responsible person to add:");
            if (int.TryParse(personInput, out int personId))
            {
                if (personId != 0 && !_dataAccess.PersonExists(personId))
                {
                    Console.WriteLine($"Person with id:{personId} Not Found. Aborting...");
                    return;
                }
            }
            var person = _dataAccess.GetOrCreatePerson(personInput);
            meeting.ResponsiblePerson = person;
            meeting.Description = _lineReader.GetStringFromLine("Description of the meeting:");

            CategoryEnum category = (CategoryEnum)_lineReader.GetCategoryFromLine();
            if (category == 0)
            {
                return;
            }
            meeting.Category = category;

            TypeEnum type = (TypeEnum)_lineReader.GetTypeFromLine();
            if (type == 0)
            {
                return;
            }
            meeting.Type = type;

            DateTime meetingDay = _lineReader.GetDateFromLine("Meeting start day (ex. '2022-06-21'):");
            TimeOnly meetingStartTime = _lineReader.GetTimeFromLine("Meeting start time (ex. 14:00):", TimeOnly.MinValue);
            TimeOnly meetingEndTime = _lineReader.GetTimeFromLine("Meeting end time (ex. 15:00):", meetingStartTime);
            meeting.StartDate = new DateTime(meetingDay.Year, meetingDay.Month, meetingDay.Day, meetingStartTime.Hour, meetingStartTime.Minute, 0);
            meeting.EndDate = new DateTime(meetingDay.Year, meetingDay.Month, meetingDay.Day, meetingEndTime.Hour, meetingEndTime.Minute, 0);

            _dataAccess.WriteMeeting(meeting);
        }

        public void GetFilteredMeetings()
        {
            var meetings = _dataAccess.GetMeetings();
            List<Meeting> filteredMeetings = new();

            int filterId = _lineReader.GetFilterIdFromLine();

            switch (filterId)
            {
                case 0:
                    return;
                case 1:
                    filteredMeetings = meetings;
                    break;
                case 2:
                    string searchDescription = _lineReader.GetStringFromLine("Type the description to filter by:");
                    filteredMeetings = meetings.Where(m => m.Description.Contains(searchDescription)).ToList();
                    break;
                case 3:
                    var responsiblePersonId = _lineReader.GetIntFromLine("Type the id of the responsible person to filter by:");
                    filteredMeetings = meetings.Where(m => m.ResponsiblePerson.Id == responsiblePersonId).ToList();
                    break;
                case 4:
                    Console.WriteLine("Type the category to filter by:");
                    var category = (CategoryEnum)_lineReader.GetCategoryFromLine();
                    filteredMeetings = meetings.Where(m => m.Category == category).ToList();
                    break;
                case 5:
                    Console.WriteLine("Type the type to filter by:");
                    var type = (TypeEnum)_lineReader.GetTypeFromLine();
                    filteredMeetings = meetings.Where(m => m.Type == type).ToList();
                    break;
                case 6:
                    Console.WriteLine("Type the number of how to filter the dates:");
                    int dateFilter = _lineReader.GetDateFilterType();
                    DateTime firstDate;
                    DateTime secondDate;
                    switch (dateFilter)
                    {
                        case 0:
                            return;
                        case 1:
                            firstDate = _lineReader.GetDateFromLine("Type the date from which to filter (e.g '2022-06-22'):");
                            filteredMeetings = meetings.Where(m => m.StartDate < firstDate).ToList();
                            break;
                        case 2:
                            firstDate = _lineReader.GetDateFromLine("Type the date until from which to filter (e.g '2022-06-22'):");
                            filteredMeetings = meetings.Where(m => m.StartDate > firstDate).ToList();
                            break;
                        case 3:
                            firstDate = _lineReader.GetDateFromLine("Type the date at what to filter (e.g '2022-06-22'):");
                            filteredMeetings = meetings.Where(m => m.StartDate.Year == firstDate.Year
                                                                   && m.StartDate.Month == firstDate.Month
                                                                   && m.StartDate.Day == firstDate.Day).ToList();
                            break;
                        case 4:
                            firstDate = _lineReader.GetDateFromLine("Type the date from what to filter (e.g '2022-06-22'):");
                            secondDate = _lineReader.GetDateFromLine("Type the date until from to filter (e.g '2022-06-23'):");
                            filteredMeetings = meetings.Where(m => m.StartDate > firstDate && m.StartDate < secondDate).ToList();
                            break;
                    }
                    break;
                case 7:
                    Console.WriteLine("Type the number of how to filter the attendees:");
                    int attendeeFilter = _lineReader.GetAttendeeFilterType();
                    int firstNum;
                    int secondNum;
                    switch (attendeeFilter)
                    {
                        case 0:
                            return;
                        case 1:
                            firstNum = _lineReader.GetIntFromLine("More than how many attendees:");
                            filteredMeetings = meetings.Where(m => m.Attendees.Count > firstNum).ToList();
                            break;
                        case 2:
                            firstNum = _lineReader.GetIntFromLine("Less than how many attendees:");
                            filteredMeetings = meetings.Where(m => m.Attendees.Count < firstNum).ToList();
                            break;
                        case 3:
                            firstNum = _lineReader.GetIntFromLine("How many exactly attendees:");
                            filteredMeetings = meetings.Where(m => m.Attendees.Count == firstNum).ToList();
                            break;
                        case 4:
                            firstNum = _lineReader.GetIntFromLine("More than how many attendees:");
                            secondNum = _lineReader.GetIntFromLine("Less than how many attendees:");
                            filteredMeetings = meetings.Where(m => m.Attendees.Count > firstNum && m.Attendees.Count < secondNum).ToList();
                            break;
                    }
                    break;
            }

            foreach (var meeting in filteredMeetings)
            {
                Console.WriteLine(meeting.ToString());
            }
        }

        public void RemoveMeeting(int userId)
        {
            int idToDelete = _lineReader.GetIntFromLine("Please provide the Id of the meeting to delete:");
            Meeting? meeting = _dataAccess.GetSingleMeeting(idToDelete);
            if (meeting is null)
            {
                Console.WriteLine($"Meeting with id:{idToDelete} Not Found. Aborting...");
                return;
            }
            else if (meeting.ResponsiblePerson.Id != userId)
            {
                Console.WriteLine($"Trying to remove a meeting by a person that is not responsible for it. Only user with id:{meeting.ResponsiblePerson.Id} can remove this. Aborting...");
                return;
            }
            _dataAccess.RemoveMeeting(idToDelete, userId);
        }

        public int Login()
        {
            int userId = _lineReader.GetIntFromLine("What is your id:");
            return userId;
        }

        public void AddPersonToMeeting()
        {
            int meetingId = _lineReader.GetIntFromLine("Id of the meeting:");

            var meeting = _dataAccess.GetSingleMeeting(meetingId);
            if(meeting is null)
            {
                Console.WriteLine($"Meeting with id:{meetingId} Not Found. Aborting...");
                return;
            }

            var personInput = _lineReader.GetStringFromLine("Name or id of the person to add:");
            if (int.TryParse(personInput, out int personId))
            {
                if (personId != 0 && !_dataAccess.PersonExists(personId))
                {
                    Console.WriteLine($"Person with id:{personId} Not Found. Aborting...");
                    return;
                }
            }

            List<Person> attendingPeople = new();
            attendingPeople.Add(meeting.ResponsiblePerson);
            if (meeting.Attendees.Count > 0)
            {
                attendingPeople.AddRange(meeting.Attendees);
            }
            else
            {
                meeting.Attendees = new List<Person>();
            }

            var personE = attendingPeople.FirstOrDefault(p => p.Id == personId);
            if (personE is not null)
            {
                Console.WriteLine($"Person {personE.Name} with id:{personE.Id} is already in the meeting. Aborting...");
                return;
            }

            var allMeetings = _dataAccess.GetMeetings();
            var personMeetings = allMeetings.Where(m => m.ResponsiblePerson.Id == personId || m.Attendees.Any(m => m.Id == personId));
            if (personMeetings.Any(m => (m.StartDate >= meeting.StartDate && m.StartDate <= meeting.EndDate) || (m.EndDate >= meeting.StartDate && m.EndDate <= meeting.EndDate)))
            {
                Console.WriteLine($"Warning! Person with id:{personId} already has a meeting at that time.");
            }

            Person personToAdd = _dataAccess.GetOrCreatePerson(personInput, personId);
            _dataAccess.AddPersonToMeeting(personToAdd, meetingId);
        }

        public void RemovePersonFromMeeting()
        {
            int meetingId = _lineReader.GetIntFromLine("Id of the meeting:");

            var meeting = _dataAccess.GetSingleMeeting(meetingId);
            if (meeting is null)
            {
                Console.WriteLine($"Meeting with id:{meetingId} Not Found. Aborting...");
                return;
            }

            var personInput = _lineReader.GetStringFromLine("Name or id of the person to remove:");
            if (int.TryParse(personInput, out int personId))
            {
                if (personId != 0 && !_dataAccess.PersonExists(personId))
                {
                    Console.WriteLine($"Person with id:{personId} Not Found. Aborting...");
                    return;
                }
                else if(personId == meeting.ResponsiblePerson.Id)
                {
                    Console.WriteLine($"Trying to remove the responsible person {meeting.ResponsiblePerson.Name} with id: {meeting.ResponsiblePerson.Id} from meeting. Aborting...");
                    return;
                }
            }
            Person personToRemove = _dataAccess.GetOrCreatePerson(personInput, personId);
            _dataAccess.RemovePersonFromMeeting(personToRemove, meetingId);
        }
    }
}
