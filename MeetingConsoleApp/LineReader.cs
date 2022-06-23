using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingConsoleApp
{
    public class LineReader : ILineReader
    {
        public string GetStringFromLine(string question)
        {
            Console.WriteLine(question);
            string? line;
            while (true)
            {
                line = Console.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Not valid input. Try again.");
                }
            }
            return line;
        }

        public int GetIntFromLine(string question)
        {
            int value;
            while (true)
            {
                string line = GetStringFromLine(question);
                if (int.TryParse(line, out value))
                {
                    if (value < 0)
                    {
                        Console.WriteLine("Number must be a positive number. Try again.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Not valid number. Try again.");
                }
            }
            return value;
        }

        public int GetCategoryFromLine()
        {
            Console.WriteLine("Category of the meeting");
            Console.WriteLine("CodeMonkey = 1");
            Console.WriteLine("Hub = 2");
            Console.WriteLine("Short = 3");
            Console.WriteLine("TeamBuilding = 4");
            Console.WriteLine("Either type the name or the assigned number to set the category for the meeting:");
            int value;
            while (true)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("Not valid input. Try again. (You can exit anytime by typing exit)");
                }
                else if (int.TryParse(line, out value))
                {
                    if (value > 0 && value < 5)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Not valid number. It must be in 1-4 range.");
                        //value = 0;
                    }
                }
                else
                {
                    line = line.ToLower();
                    switch (line)
                    {
                        case "exit":
                            return 0;
                        case "codemonkey":
                            value = 1;
                            break;
                        case "hub":
                            value = 2;
                            break;
                        case "short":
                            value = 3;
                            break;
                        case "teambuilding":
                            value = 4;
                            break;
                        default:
                            Console.WriteLine("Unrecognized text");
                            break;
                    }
                    if (value != 0)
                    {
                        break;
                    }

                }
            }
            return value;
        }

        public int GetTypeFromLine()
        {
            Console.WriteLine("Type of the meeting");
            Console.WriteLine("Live = 1");
            Console.WriteLine("InPerson = 2");
            Console.WriteLine("Either type the name or the assigned number to set the category for the meeting:");

            int value;
            while (true)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("Not valid input. Try again. (You can exit anytime by typing exit)");
                }
                else if (int.TryParse(line, out value))
                {
                    if (value > 0 && value < 3)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Not valid number. It must be in 1-2 range.");
                        //value = 0;
                    }
                }
                else
                {
                    line = line.ToLower();
                    switch (line)
                    {
                        case "exit":
                            return 0;
                        case "inperson":
                            value = 1;
                            break;
                        case "live":
                            value = 2;
                            break;
                        default:
                            Console.WriteLine("Unrecognized text, try again.");
                            break;
                    }
                    if (value != 0)
                    {
                        break;
                    }
                }
            }
            return value;
        }

        public int GetFilterIdFromLine()
        {
            Console.WriteLine("1 - Get all meetings.");
            Console.WriteLine("2 - Filter by description.");
            Console.WriteLine("3 - Filter by responsible person.");
            Console.WriteLine("4 - Filter by category.");
            Console.WriteLine("5 - Filter by type.");
            Console.WriteLine("6 - Filter by dates.");
            Console.WriteLine("7 - Filter by the number of attendees");
            Console.WriteLine("Type the number of the filter you want to use:");
            int value;

            while (true)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("Not valid input. Try again. (You can exit anytime by typing exit)");
                }
                else if (line.Equals("exit"))
                {
                    return 0;
                }
                else if (int.TryParse(line, out value))
                {
                    if (value > 0 && value < 8)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Not valid number. It must be in 1-7 range.");
                        value = 0;
                    }
                }
            }
            return value;
        }

        public int GetDateFilterType()
        {
            Console.WriteLine("1 - From 1 date");
            Console.WriteLine("2 - Before 1 date");
            Console.WriteLine("3 - At exact 1 date");
            Console.WriteLine("4 - Between 2 dates");
            Console.WriteLine("Type the number of the filter you want to use:");
            int value;
            while (true)
            {
                var line = Console.ReadLine();


                if (!string.IsNullOrEmpty(line))
                {
                    if (line.Equals("exit"))
                    {
                        return 0;
                    }
                    else if (int.TryParse(line, out value))
                    {
                        if (value > 0 && value < 5)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Not valid number. It must be in 1-4 range.");
                            value = 0;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Not valid input. Try again. (You can exit anytime by typing exit)");
                }

            }
            return value;
        }

        public int GetAttendeeFilterType()
        {
            Console.WriteLine("1 - More than x attendees.");
            Console.WriteLine("2 - Less than x attendees.");
            Console.WriteLine("3 - Exactly x of attendees.");
            Console.WriteLine("4 - Between x and y of attendees.");
            Console.WriteLine("Type the number of the filter you want to use:");
            int value;
            while (true)
            {
                var line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.Equals("exit"))
                    {
                        return 0;
                    }
                    else if (int.TryParse(line, out value))
                    {
                        if (value > 0 && value < 5)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Not valid number. It must be in 1-4 range.");
                            value = 0;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Not valid input. Try again. (You can exit anytime by typing exit)");
                }

            }
            return value;
        }

        public DateTime GetDateFromLine(string question)
        {
            string line = GetStringFromLine(question);
            DateTime value;
            while (true)
            {
                if (DateTime.TryParse(line, out value))
                {
                    break;
                }
                else
                {
                    line = GetStringFromLine("Not valid date. Try again.");
                }
            }
            value = new DateTime(value.Year, value.Month, value.Day); // we are only asking for the day
            return value;
        }

        public TimeOnly GetTimeFromLine(string question, TimeOnly start)
        {
            string line = GetStringFromLine(question);
            TimeOnly value;
            while (true)
            {
                if (TimeOnly.TryParse(line, out value))
                {
                    if (value > start)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Meeting cannot end before the meeting starts. Type a time that is after {start}");
                        line = GetStringFromLine("Try again.");
                    }
                }
                else
                {
                    line = GetStringFromLine("Not valid time. Try again.");
                }
            }
            value = new TimeOnly(value.Hour, value.Minute); // we are only asking for the time
            return value;
        }
    }
}
