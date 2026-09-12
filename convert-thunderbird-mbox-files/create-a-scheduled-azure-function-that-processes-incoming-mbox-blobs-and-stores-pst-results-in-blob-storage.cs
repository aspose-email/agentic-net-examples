using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        const string inputMboxPath = "input.mbox";
        const string outputPstPath = "output.pst";

        // Ensure a placeholder MBOX file exists.
        if (!File.Exists(inputMboxPath))
        {
            // Create a minimal MBOX file with a single empty message.
            string placeholderMessage = "From - Mon Jan 01 00:00:00 2024\r\nSubject: Placeholder\r\n\r\nThis is a placeholder message.\r\n\r\n";
            File.WriteAllText(inputMboxPath, placeholderMessage);
            Console.WriteLine($"Created placeholder MBOX file at '{inputMboxPath}'.");
        }

        try
        {
            // Convert MBOX to PST.
            MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath);
            Console.WriteLine($"Successfully converted '{inputMboxPath}' to PST at '{outputPstPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}
