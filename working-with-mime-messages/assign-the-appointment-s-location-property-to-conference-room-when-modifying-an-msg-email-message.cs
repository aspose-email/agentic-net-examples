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
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputMsgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the existing MSG file
            using (MapiMessage originalMessage = MapiMessage.Load(inputMsgPath))
            {
                // Create a minimal appointment (subject, dates, organizer, attendees)
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));

                Appointment appointment = new Appointment(
                    "Sample Meeting",
                    new DateTime(2024, 12, 1, 10, 0, 0),
                    new DateTime(2024, 12, 1, 11, 0, 0),
                    organizer,
                    attendees);

                // Set the desired location
                appointment.Location = "Conference Room";

                // Convert the appointment to a MapiMessage
                using (MapiMessage updatedMessage = appointment.ToMapiMessage())
                {
                    // Preserve original message properties (subject, body, etc.) if needed
                    updatedMessage.Subject = originalMessage.Subject;
                    updatedMessage.Body = originalMessage.Body;

                    // Save the modified message
                    updatedMessage.Save(outputMsgPath);
                }
            }

            Console.WriteLine($"Message saved to {outputMsgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
