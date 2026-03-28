using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "appointment.msg";

            // Guard file existence
            if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file as a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
            {
                // Prepare conversion options (default)
                MailConversionOptions conversionOptions = new MailConversionOptions();

                // Convert to MailMessage
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Assume the body contains the iCalendar data
                    string icsContent = mailMessage.Body;

                    if (string.IsNullOrWhiteSpace(icsContent))
                    {
                        Console.Error.WriteLine("The MSG file does not contain iCalendar data in the body.");
                        return;
                    }

                    // Load the appointment from the iCalendar content
                    using (MemoryStream icsStream = new MemoryStream(Encoding.UTF8.GetBytes(icsContent)))
                    {
                        Appointment appointment = Appointment.Load(icsStream);

                        // Use the appointment (example: display some properties)
                        Console.WriteLine($"Summary: {appointment.Summary}");
                        Console.WriteLine($"Location: {appointment.Location}");
                        Console.WriteLine($"Start: {appointment.StartDate}");
                        Console.WriteLine($"End: {appointment.EndDate}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
