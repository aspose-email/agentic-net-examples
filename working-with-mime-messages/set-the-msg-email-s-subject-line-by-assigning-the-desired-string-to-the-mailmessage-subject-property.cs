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
            // Define input and output MSG file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";

            // Verify that the input file exists
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

            // Load the MSG file into a MapiMessage
            MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

            // Convert MapiMessage to MailMessage for easy manipulation
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions))
            {
                // Set the desired subject line
                mailMsg.Subject = "Updated Subject Line";

                // Save the modified message back to MSG format
                mailMsg.Save(outputMsgPath);
            }

            Console.WriteLine($"Message saved with new subject to: {outputMsgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
