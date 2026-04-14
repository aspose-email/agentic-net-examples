using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "appointment.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create appointment details
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            DateTime start = new DateTime(2023, 12, 25, 10, 0, 0);
            DateTime end = start.AddHours(1);

            // Initialize the appointment
            Appointment appointment = new Appointment("Conference Room", start, end, organizer, attendees)
            {
                Summary = "Year End Meeting",
                Description = "Discuss yearly results."
            };

            // Set the privacy flag to Confidential
            appointment.Class = AppointmentClass.Confidential;

            // Save the appointment as MSG
            AppointmentMsgSaveOptions saveOptions = new AppointmentMsgSaveOptions();
            appointment.Save(msgPath, saveOptions);

            // Verify that the MSG file was created
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("Failed to create the MSG file.");
                return;
            }

            Console.WriteLine("Appointment saved to MSG successfully.");

            // Load the MSG file as a MapiMessage to confirm the flag
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Retrieve the appointment class property
                var classProperty = mapiMessage.GetProperty(KnownPropertyList.AppointmentMessageClass);
                Console.WriteLine("Stored appointment class property: " + (classProperty?.ToString() ?? "null"));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
