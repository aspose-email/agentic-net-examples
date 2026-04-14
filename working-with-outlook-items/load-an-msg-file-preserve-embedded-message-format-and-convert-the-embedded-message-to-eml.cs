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

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputMsgPath))
            {
                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = "sender@example.com";
                    placeholder.To.Add("receiver@example.com");
                    placeholder.Subject = "Placeholder MSG";
                    placeholder.Body = "This is a placeholder MSG file generated because the original file was missing.";

                    MsgSaveOptions placeholderSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                    {
                        PreserveOriginalDates = true
                    };

                    placeholder.Save(inputMsgPath, placeholderSaveOptions);
                }
            }

            // Load the MSG file while preserving the format of any embedded messages.
            MsgLoadOptions loadOptions = new MsgLoadOptions()
            {
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputMsgPath, loadOptions))
            {
                // Save the message as EML, preserving the original format of embedded messages.
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                message.Save(outputEmlPath, emlSaveOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
