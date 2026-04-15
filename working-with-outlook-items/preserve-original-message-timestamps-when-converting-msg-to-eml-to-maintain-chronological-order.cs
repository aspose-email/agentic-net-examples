using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Guard input file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
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

            // Load MSG file into MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputMsgPath))
            {
                // Save as EML preserving original timestamps (default behavior)
                mailMessage.Save(outputEmlPath, SaveOptions.DefaultEml);
                Console.WriteLine($"Converted '{inputMsgPath}' to '{outputEmlPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
