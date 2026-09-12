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
            // Author note: Simple console app to convert a calendar MSG to iCalendar (ICS) preserving recurrence.
            string inputMsgPath = "input.msg";
            string outputIcsPath = "output.ics";

            // Guard file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapMsg;
            try
            {
                mapMsg = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Convert the MAPI message to a MailMessage (which can represent a calendar item)
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage mailMessage;
            try
            {
                mailMessage = mapMsg.ToMailMessage(conversionOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion to MailMessage failed: {ex.Message}");
                return;
            }

            // Save as iCalendar (.ics). The library infers the format from the file extension.
            try
            {
                mailMessage.Save(outputIcsPath);
                Console.WriteLine($"Successfully saved iCalendar file: {outputIcsPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save iCalendar file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
