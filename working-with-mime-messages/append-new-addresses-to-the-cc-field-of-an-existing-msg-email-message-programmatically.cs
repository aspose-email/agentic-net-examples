using System;
using System.IO;
using Aspose.Email;

namespace AppendCcToMsg
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the existing MSG file
                string inputPath = "input.msg";
                // Path where the updated MSG file will be saved
                string outputPath = "output.msg";

                // Verify that the input file exists before attempting to load it
                if (!File.Exists(inputPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


                // Load the MSG file into a MailMessage instance
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Append new addresses to the CC collection
                    message.CC.Add(new MailAddress("alice@example.com"));
                    message.CC.Add(new MailAddress("bob@example.com"));

                    // Save the modified message back to a MSG file
                    try
                    {
                        message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        Console.WriteLine($"Message saved successfully to: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save the message: {saveEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
