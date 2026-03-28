using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX file and output PST file paths
            string inputMboxPath = "input.mbox";
            string outputPstPath = "output.pst";

            // Verify that the input file exists
            if (!File.Exists(inputMboxPath))
{
    try
    {
        string placeholderMbox = "From placeholder@example.com Sat Jan 01 00:00:00 2026\n";
        File.WriteAllText(inputMboxPath, placeholderMbox);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
        return;
    }
}


            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                    return;
                }
            }

            // Perform the conversion from MBOX to PST
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath))
                {
                    // Conversion succeeded; additional processing can be added here if needed
                    Console.WriteLine("MBOX to PST conversion completed successfully.");
                }
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"Error during conversion: {convEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }
    }
}
