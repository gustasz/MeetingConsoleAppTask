using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingConsoleApp.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Person ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public CategoryEnum Category { get; set; }
        public TypeEnum Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<Person> Attendees { get; set; }

        public override string ToString()
        {
            var meetingInfo = $@"
 Id:{Id} 
 Name:{Name} 
 Responsible Person:
    Id:{ResponsiblePerson.Id}    Name: {ResponsiblePerson.Name} 
 Description: {Description} 
 Category:{Category}
 Type:{Type}
 StartDate: {StartDate}
 EndDate: {EndDate}
 Attendee count: {Attendees.Count}
";
            string attendeeInfo = "";
            if (Attendees.Count > 0)
            {
                List<string> attendeeStrings = new();
                foreach(var attendee in Attendees)
                {
                    attendeeStrings.Add(attendee.ToString());
                }
                attendeeInfo = String.Join(", ", attendeeStrings);
            }
            return meetingInfo + attendeeInfo;
    }
    }
}
