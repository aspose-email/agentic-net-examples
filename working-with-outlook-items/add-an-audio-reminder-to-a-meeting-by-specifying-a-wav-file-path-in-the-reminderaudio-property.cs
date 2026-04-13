using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the audio reminder and the output MSG file
            string wavPath = "reminder.wav";
            string outputMsgPath = "MeetingWithAudioReminder.msg";

            // Ensure the WAV file exists; create an empty placeholder if missing
            if (!File.Exists(wavPath))
            {
                try
                {
                    File.WriteAllBytes(wavPath, new byte[0]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder WAV file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create a MAPI calendar (meeting) and configure the audio reminder
            using (MapiCalendar meeting = new MapiCalendar(
                location: "Conference Room",
                summary: "Team Sync",
                description: "Weekly team synchronization meeting.",
                startDate: DateTime.Now.AddHours(1),
                endDate: DateTime.Now.AddHours(2)))
            {
                // Enable reminder and set it to fire 15 minutes before the start
                meeting.ReminderSet = true;
                meeting.ReminderDelta = 15; // minutes

                // Specify the full path to the WAV file to be played as the reminder audio
                meeting.ReminderFileParameter = Path.GetFullPath(wavPath);

                // Save the meeting as an MSG file using the default MSG save options
                try
                {
                    meeting.Save(outputMsgPath, MapiCalendarSaveOptions.DefaultMsg);
                    Console.WriteLine($"Meeting with audio reminder saved to: {outputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save meeting: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
