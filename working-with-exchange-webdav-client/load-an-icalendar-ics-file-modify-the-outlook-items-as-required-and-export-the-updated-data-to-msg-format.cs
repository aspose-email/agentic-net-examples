using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace AsposeEmailIcsToMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputIcsPath = "input.ics";
                string outputMsgPath = "output.msg";

                // Ensure input file exists; create minimal placeholder if missing
                if (!File.Exists(inputIcsPath))
                {
                    try
                    {
                        string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                        File.WriteAllText(inputIcsPath, placeholderIcs);
                        Console.WriteLine($"Placeholder iCalendar file created at '{inputIcsPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
                        return;
                    }
                }

                // Ensure output directory exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                    return;
                }

                // Load the iCalendar file
                Appointment appointment;
                try
                {
                    appointment = Appointment.Load(inputIcsPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load iCalendar file: {ex.Message}");
                    return;
                }

                // Modify appointment properties as required
                appointment.Summary = "Updated Meeting";
                appointment.Location = "Conference Room";

                // Convert to MAPI message and save as MSG
                try
                {
                    using (MapiMessage mapMessage = appointment.ToMapiMessage())
                    {
                        mapMessage.Save(outputMsgPath);
                    }
                    Console.WriteLine($"MSG file saved to '{outputMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert or save MSG file: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
