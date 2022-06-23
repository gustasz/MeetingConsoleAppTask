using MeetingConsoleApp.Data;
using MeetingConsoleApp.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MeetingConsoleApp.Tests
{
    public class CmdHelperTests
    {
        private readonly CmdHelper _sut;
        private readonly Mock<ILineReader> _lineReaderMock = new();
        private readonly Mock<IDataAccess> _dataAccessMock = new();
        public CmdHelperTests()
        {
            _sut = new(_dataAccessMock.Object, _lineReaderMock.Object);
        }

        [Fact]
        public void RemoveMeeting_ReturnsError_IfPersonNotResponsibleTriesToRemove()
        {
            //Arrange
            int userId = 2;
            int meetingId = 4;
            Person personResponsible = new() { Id = 3, Name = "Jimmy" };
            Meeting foundMeeting = new() { Id = meetingId, ResponsiblePerson = personResponsible };
            _lineReaderMock.Setup(x => x.GetIntFromLine(It.IsAny<string>()))
                 .Returns(meetingId);
            _dataAccessMock.Setup(x => x.GetSingleMeeting(meetingId))
                .Returns(foundMeeting);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            //Act
            _sut.RemoveMeeting(userId); // user with id:2 trying to remove a meeting by responsible person id:3
            //Assert
            Assert.StartsWith("Trying to remove a meeting by a person that is not responsible", stringWriter.ToString());
        }

        [Fact]
        public void AddPersonToMeeting_GivesWarning_IfPersonAlreadyInAMeetingThatIntersects()
        {
            //Arrange
            int personId = 4;
            int meetingId = 5;
            Person personResponsible = new() { Id = 3, Name = "Jimmy" };
            Person randomPerson = new() { Id = 4, Name = "Timmy" };
            List<Person> foundMeetingAttendees = new();
            foundMeetingAttendees.Add(randomPerson);
            Meeting foundMeeting = new() { Id = meetingId, StartDate = new DateTime(2022, 02, 02, 10, 30, 0), EndDate = new DateTime(2022, 02, 02, 11, 30, 0), ResponsiblePerson = personResponsible, Attendees = new List<Person>()};
            List<Person> anotherMeetingAttendees = new();
            anotherMeetingAttendees.Add(randomPerson);
            Meeting meetingAlreadyIn = new() { Id = 8, StartDate = new DateTime(2022,02,02,10,0,0), EndDate = new DateTime(2022,02,02,11,0,0), ResponsiblePerson = personResponsible, Attendees = anotherMeetingAttendees };

            List<Meeting> allMeetings = new();
            allMeetings.Add(meetingAlreadyIn);
            allMeetings.Add(foundMeeting);

            _lineReaderMock.Setup(x => x.GetIntFromLine(It.IsAny<string>()))
                 .Returns(meetingId);
            _dataAccessMock.Setup(x => x.GetSingleMeeting(meetingId))
                .Returns(foundMeeting);
            _lineReaderMock.Setup(x => x.GetStringFromLine(It.IsAny<string>()))
                .Returns("4");
            _dataAccessMock.Setup(x => x.PersonExists(personId))
                .Returns(true);
            _dataAccessMock.Setup(x => x.GetMeetings())
                .Returns(allMeetings);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            //Act
            _sut.AddPersonToMeeting(); // adding person to a meeting that is on 2022-02-02 10:30 to 11:30, when the person already has a meeting on 2022-02-02 from 10:00 to 11:00
            //Assert
            Assert.StartsWith($"Warning! Person with id:{personId} already has a meeting", stringWriter.ToString());
        }

        [Fact]
        public void AddPersonToMeeting_ReturnsError_IfSamePersonIsBeingAddedTwice()
        {
            //Arrange
            int meetingId = 2;
            int personId = 3;
            Person personResponsible = new() { Id = 3, Name = "Jimmy" };
            Meeting meeting = new() { Id = meetingId, ResponsiblePerson = personResponsible, Attendees = new List<Person>() };
            _lineReaderMock.Setup(x => x.GetIntFromLine(It.IsAny<string>()))
                 .Returns(meetingId);
            _dataAccessMock.Setup(x => x.GetSingleMeeting(meetingId))
                .Returns(meeting);
            _lineReaderMock.Setup(x => x.GetStringFromLine(It.IsAny<string>()))
                .Returns("3");
            _dataAccessMock.Setup(x => x.PersonExists(personId))
                .Returns(true);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            //Act
            _sut.AddPersonToMeeting(); // adding a person who is already responsible for the meeting to the meeting
            //Assert
            Assert.StartsWith("Person Jimmy with id:3 is already in the meeting.", stringWriter.ToString());
        }

        [Fact]
        public void RemovePersonFromMeeting_ReturnsError_IfPersonRemovedIsResponsiblePerson()
        {
            //Arrange
            int meetingId = 2;
            int personId = 3;
            Person personResponsible = new() { Id = 3, Name = "Jimmy" };
            Meeting meeting = new() { Id = meetingId, ResponsiblePerson = personResponsible, Attendees = new List<Person>() };
            _lineReaderMock.Setup(x => x.GetIntFromLine(It.IsAny<string>()))
                 .Returns(meetingId);
            _dataAccessMock.Setup(x => x.GetSingleMeeting(meetingId))
                .Returns(meeting);
            _lineReaderMock.Setup(x => x.GetStringFromLine(It.IsAny<string>()))
                .Returns("3");
            _dataAccessMock.Setup(x => x.PersonExists(personId))
                .Returns(true);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            //Act
            _sut.RemovePersonFromMeeting(); // trying to remove the responsible person from the meeting
            //Assert
            Assert.StartsWith("Trying to remove the responsible person", stringWriter.ToString());
        }
    }
}