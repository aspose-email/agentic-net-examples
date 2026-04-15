using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgValidation
{
    class Program
    {
        static void Main()
        {
            try
            {
                string msgPath = "sample.msg";

                // Guard against missing file
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

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    return;
                }

                // Load the MSG file
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Validate Subject
                    if (string.IsNullOrWhiteSpace(message.Subject))
                    {
                        Console.WriteLine("Subject is missing.");
                    }
                    else
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }

                    // Validate Recipients
                    bool hasToRecipient = false;
                    foreach (MapiRecipient recipient in message.Recipients)
                    {
                        if (recipient.RecipientType == MapiRecipientType.MAPI_TO)
                        {
                            hasToRecipient = true;
                            Console.WriteLine($"To: {recipient.EmailAddress}");
                        }
                        else if (recipient.RecipientType == MapiRecipientType.MAPI_CC)
                        {
                            Console.WriteLine($"Cc: {recipient.EmailAddress}");
                        }
                        else if (recipient.RecipientType == MapiRecipientType.MAPI_BCC)
                        {
                            Console.WriteLine($"Bcc: {recipient.EmailAddress}");
                        }
                    }

                    if (!hasToRecipient)
                    {
                        Console.WriteLine("No primary (To) recipients found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
