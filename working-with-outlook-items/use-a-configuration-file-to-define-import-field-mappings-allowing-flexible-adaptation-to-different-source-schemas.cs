using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the JSON configuration that defines source-to-target field mappings
            string configPath = "fieldMapping.json";
            if (!File.Exists(configPath))
            {
                Console.Error.WriteLine($"Configuration file not found: {configPath}");
                return;
            }

            // Path to the source email file (EML format)
            string emailPath = "source.eml";
            if (!File.Exists(emailPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emailPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source email file not found: {emailPath}");
                return;
            }

            // Load field mappings from the configuration file
            Dictionary<string, string> fieldMapping;
            try
            {
                using (FileStream configStream = File.OpenRead(configPath))
                {
                    fieldMapping = JsonSerializer.Deserialize<Dictionary<string, string>>(configStream);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read configuration: {ex.Message}");
                return;
            }

            // Load the source email into a MailMessage object
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emailPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email: {ex.Message}");
                return;
            }

            if (mailMessage == null)
            {
                Console.Error.WriteLine("Loaded MailMessage is null.");
                return;
            }

            using (mailMessage)
            {
                // Convert MailMessage to MapiMessage
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);
                using (mapiMessage)
                {
                    // Apply mappings defined in the configuration
                    foreach (KeyValuePair<string, string> map in fieldMapping)
                    {
                        string sourceField = map.Key;
                        string targetProperty = map.Value;

                        // Example mappings – extend as needed
                        if (sourceField.Equals("From", StringComparison.OrdinalIgnoreCase) &&
                            targetProperty.Equals("SenderEmailAddress", StringComparison.OrdinalIgnoreCase))
                        {
                            if (mailMessage.From != null)
                            {
                                mapiMessage.SenderEmailAddress = mailMessage.From.Address;
                            }
                        }
                        else if (sourceField.Equals("Subject", StringComparison.OrdinalIgnoreCase) &&
                                 targetProperty.Equals("Subject", StringComparison.OrdinalIgnoreCase))
                        {
                            mapiMessage.Subject = mailMessage.Subject;
                        }
                        else if (sourceField.Equals("Body", StringComparison.OrdinalIgnoreCase) &&
                                 targetProperty.Equals("Body", StringComparison.OrdinalIgnoreCase))
                        {
                            mapiMessage.Body = mailMessage.Body;
                        }
                        // Additional field mappings can be added here following the same pattern
                    }

                    // Save the resulting MAPI message to a MSG file
                    string outputPath = "output.msg";
                    try
                    {
                        mapiMessage.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
