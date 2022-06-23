using MeetingConsoleApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetingConsoleApp.Data
{
    public class DataAccess : IDataAccess
    {
        readonly string fileName = "MeetingsData.json";

        public void CreateEmptyFile()
        {
            File.Create(fileName).Dispose();
        }
        public bool FileExists()
        {
            if (File.Exists(fileName))
                return true;
            else
                return false;
        }
        public List<Meeting> GetMeetings()
        {
            List<Meeting> meetings = new();
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);
                    if (data is null)
                    {
                        data = new List<Meeting>();
                    }
                    meetings = data;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
            return meetings;
        }

        public void WriteMeeting(Meeting meeting)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);
                    if (data is null)
                    {
                        data = new List<Meeting>();
                    }

                    if (data.Count > 0)
                    {
                        int highestId = data.OrderByDescending(m => m.Id).First().Id;
                        meeting.Id = highestId + 1;
                    }
                    else
                    {
                        meeting.Id = 1;
                    }
                    meeting.Attendees = new List<Person>();
                    data.Add(meeting);
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
                    Console.WriteLine($"Meeting with id:{meeting.Id} succesfully added.");
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
        }

        public Meeting? GetSingleMeeting(int id)
        {
            Meeting? meeting = new();
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);
                    if (data is null)
                    {
                        data = new List<Meeting>();
                    }
                    meeting = data.FirstOrDefault(x => x.Id == id);
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
            return meeting;
        }

        public void RemoveMeeting(int id, int userId)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);
                    if (data is null)
                    {
                        data = new List<Meeting>();
                    }
                    var meeting = data.FirstOrDefault(x => x.Id == id);
                    if (meeting is null)
                    {
                        Console.WriteLine($"Meeting with id:{id} Not Found. Aborting...");
                        return;
                    }

                    data.Remove(meeting);
                    Console.WriteLine($"Meeting with id:{meeting.Id} succesfully deleted.");

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
        }

        public List<Person> GetPeople()
        {
            List<Person> people = new();
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);

                    if (data is null)
                    {
                        return people;
                    }

                    foreach (var meeting in data)
                    {
                        List<Person> associatedPeople = new();
                        associatedPeople.Add(meeting.ResponsiblePerson);
                        if (meeting.Attendees is not null)
                        {
                            associatedPeople.AddRange(meeting.Attendees);
                        }

                        people = people.Union(associatedPeople).ToList(); // get a new list that contains only distinct people
                    }

                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
            return people;
        }

        public bool PersonExists(int id)
        {
            var allPeople = GetPeople();
            if (allPeople.Any(p => p.Id == id))
            {
                return true;
            }
            return false;
        }

        public bool MeetingExists(int id)
        {
            var allMeetings = GetMeetings();
            if (allMeetings.Any(p => p.Id == id))
            {
                return true;
            }
            return false;
        }
        public Person GetOrCreatePerson(string name, int id = 0)
        {
            var allPeople = GetPeople();

            Person? person;

            if (allPeople.Count == 0)
            {
                Console.WriteLine("Person with this name or id was not found. Creating new person...");
                person = new Person { Id = 1, Name = name };
                return person;
            }

            if (id is not 0)
            {
                person = allPeople.FirstOrDefault(p => p.Id == id);
                if (person is not null)
                {
                    return person;
                }
            }

            var personWithSameName = allPeople.Where(p => p.Name == name);
            if (personWithSameName.Any())
            {
                person = personWithSameName.First();
                return person;
            }

            Console.WriteLine("Person with this name or id was not found. Creating new person...");
            int highestId = allPeople.OrderByDescending(p => p.Id).First().Id; // get the highest id already existing
            person = new Person { Id = highestId + 1, Name = name };
            return person;
        }

        public void AddPersonToMeeting(Person person, int meetingId)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);
                    if (data is null)
                    {
                        data = new List<Meeting>();
                    }
                    var meeting = data.FirstOrDefault(m => m.Id == meetingId);
                    if (meeting is null)
                    {
                        Console.WriteLine($"Meeting with id:{meetingId} Not Found. Aborting...");
                        return;
                    }

                    data.Remove(meeting);
                    meeting.Attendees.Add(person);
                    data.Add(meeting);

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
                    Console.WriteLine($"Person with id:{person.Id} succesfully added at {DateTime.Now}");
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
        }

        public void RemovePersonFromMeeting(Person person, int meetingId)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<List<Meeting>>(jsonString);

                    if (data is null)
                    {
                        data = new List<Meeting>();
                    }

                    var meeting = data.FirstOrDefault(m => m.Id == meetingId);

                    if (meeting is null)
                    {
                        Console.WriteLine($"Meeting with id:{meetingId} Not Found. Aborting...");
                        return;
                    }

                    List<Person> attendingPeople = new();
                    attendingPeople = meeting.Attendees.ToList();

                    if (!attendingPeople.Any(p => p.Id == person.Id))
                    {
                        Console.WriteLine($"Person {person.Name} with id:{person.Id} not found in the meeting. Aborting...");
                        return;
                    }

                    data.Remove(meeting);
                    var personToRemove = meeting.Attendees.FirstOrDefault(p => p.Id == person.Id);
                    if(personToRemove is not null)
                    {
                        meeting.Attendees.Remove(personToRemove);
                    }                   
                    data.Add(meeting);

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
                    Console.WriteLine($"Person with id:{person.Id} succesfully removed.");
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Meetings file could not be opened");
            }
        }
    }
}
