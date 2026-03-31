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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
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

                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder MSG file.";
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file.
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                // Add To recipient.
                msg.Recipients.Add("to@example.com", "To Recipient", MapiRecipientType.MAPI_TO);

                // Add Cc recipient.
                msg.Recipients.Add("cc@example.com", "Cc Recipient", MapiRecipientType.MAPI_CC);

                // Add Bcc recipient.
                msg.Recipients.Add("bcc@example.com", "Bcc Recipient", MapiRecipientType.MAPI_BCC);

                // Save the updated MSG file.
                try
                {
                    msg.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
