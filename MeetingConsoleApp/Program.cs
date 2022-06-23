using MeetingConsoleApp;
using MeetingConsoleApp.Data;
using MeetingConsoleApp.Models;
using System.Text.Json;


string cmd;
int userId = 0;
LineReader lineReader = new();
DataAccess dataAccess = new();
CmdHelper cmdHelper = new(dataAccess, lineReader);

if (!dataAccess.FileExists())
{
    dataAccess.CreateEmptyFile();
}

bool programActive = true;
while (programActive)
{
    cmd = lineReader.GetStringFromLine("Enter a command (help for a list of commands):");
    switch (cmd)
    {
        case "meeting create":
            cmdHelper.CreateMeeting();
            break;
        case "meeting delete":
            cmdHelper.RemoveMeeting(userId);
            break;
        case "meeting add person":
            cmdHelper.AddPersonToMeeting();
            break;
        case "meeting remove person":
            cmdHelper.RemovePersonFromMeeting();
            break;
        case "meeting get":
            cmdHelper.GetFilteredMeetings();
            break;
        case "login":
            userId = cmdHelper.Login();
            break;
        case "help":
            Console.WriteLine("Available commands:");
            Console.WriteLine("meeting create; meeting delete; meeting add person; meeting remove person; meeting get; login; help; exit;");
            break;
        case "exit":
            Console.WriteLine("Exiting the program...");
            programActive = false;
            break;
    }
}

