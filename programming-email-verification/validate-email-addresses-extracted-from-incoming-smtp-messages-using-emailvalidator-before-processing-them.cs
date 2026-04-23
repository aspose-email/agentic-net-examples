using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the incoming SMTP message saved as an EML file
            string emlPath = "incoming.eml";

            // Guard file existence
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            // Load the message safely
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Create the validator instance
                EmailValidator validator = new EmailValidator();

                // Validate each recipient address
                foreach (MailAddress recipient in message.To)
                {
                    ValidationResult validationResult;
                    validator.Validate(recipient.Address, out validationResult);

                    if (validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess)
                    {
                        Console.WriteLine($"Valid address: {recipient.Address}");
                        // Proceed with processing for this address
                    }
                    else
                    {
                        Console.WriteLine($"Invalid address: {recipient.Address} - Reason: {validationResult.Message}");
                        // Skip or handle invalid address as needed
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
