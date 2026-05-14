using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the audio file that will be used for the reminder
            string audioFilePath = "reminder.wav";

            // Verify that the audio file exists before proceeding
            if (!File.Exists(audioFilePath))
            {
                Console.Error.WriteLine($"Audio file not found: {audioFilePath}");
                return;
            }

            // Prepare attendees for the meeting
            MailAddressCollection attendees = new MailAddressCollection
            {
                new MailAddress("alice@example.com"),
                new MailAddress("bob@example.com")
            };

            // Create a new appointment (meeting)
            Appointment meeting = new Appointment(
                location: "Conference Room",
                summary: "Project Sync",
                description: "Discuss project status and next steps.",
                startDate: new DateTime(2024, 12, 1, 10, 0, 0),
                endDate: new DateTime(2024, 12, 1, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees);

            // Create a reminder for the appointment
            AppointmentReminder reminder = new AppointmentReminder();
            reminder.Action = ReminderAction.Audio; // Use audio action

            // Attach the WAV file as a reminder attachment
            Uri audioUri = new Uri(Path.GetFullPath(audioFilePath));
            reminder.Attachments.Add(new ReminderAttachment(audioUri));

            // Add the reminder to the appointment
            meeting.Reminders.Add(reminder);

            // Save the appointment to an iCalendar file
            string outputPath = "meeting.ics";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            meeting.Save(outputPath);
            Console.WriteLine($"Appointment saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
