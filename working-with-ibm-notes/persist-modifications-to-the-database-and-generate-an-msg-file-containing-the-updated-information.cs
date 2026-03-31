using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string dbPath = "appointments.txt";
            string msgPath = "appointment.msg";

            // Ensure the database file exists (create minimal placeholder if missing)
            if (!File.Exists(dbPath))
            {
                try
                {
                    File.WriteAllText(dbPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create database file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the MSG file exists
            string msgDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(msgDirectory) && !Directory.Exists(msgDirectory))
            {
                try
                {
                    Directory.CreateDirectory(msgDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory for MSG file: {ex.Message}");
                    return;
                }
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Conference Room 1",
                new DateTime(2024, 5, 20, 10, 0, 0),
                new DateTime(2024, 5, 20, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timelines.";

            // Optionally set a daily recurrence pattern
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(5, 1);
            recurrence.Interval = 1;
            appointment.Recurrence = recurrence;

            // Persist appointment details to the "database" (text file)
            string dbEntry = $"{appointment.StartDate:u}|{appointment.EndDate:u}|{appointment.Summary}|{appointment.Description}";
            try
            {
                File.AppendAllText(dbPath, dbEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write to database file: {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message and save as MSG
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create MSG file: {ex.Message}");
                return;
            }

            Console.WriteLine("Appointment saved to database and MSG file successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
