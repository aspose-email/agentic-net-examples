using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                location: "Conference Room",
                summary: "Project Kickoff",
                description: "Discuss project goals and timelines.",
                startDate: new DateTime(2023, 10, 1, 10, 0, 0),
                endDate: new DateTime(2023, 10, 1, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees);

            // Set importance to High
            appointment.MicrosoftImportance = MSImportance.High;

            // Export to iCalendar file
            string icsPath = "appointment.ics";
            appointment.Save(icsPath, AppointmentSaveFormat.Ics);

            // Verify that the importance flag appears in the exported file
            if (File.Exists(icsPath))
            {
                try
                {
                    string content = File.ReadAllText(icsPath);
                    if (content.Contains("HIGH"))
                    {
                        Console.WriteLine("Importance flag set to High and verified in the iCalendar file.");
                    }
                    else
                    {
                        Console.WriteLine("Importance flag not found in the iCalendar file.");
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error reading the iCalendar file: {ioEx.Message}");
                }
            }
            else
            {
                Console.Error.WriteLine("Failed to create the iCalendar file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
