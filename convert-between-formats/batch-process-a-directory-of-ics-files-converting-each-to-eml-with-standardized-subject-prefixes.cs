using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "ics";
            string outputDirectory = "eml";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Get all .ics files in the input directory
            string[] icsFiles;
            try
            {
                icsFiles = Directory.GetFiles(inputDirectory, "*.ics");
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Failed to enumerate .ics files: {fileEx.Message}");
                return;
            }

            foreach (string icsPath in icsFiles)
            {
                try
                {
                    // Guard against missing file
                    if (!File.Exists(icsPath))
                    {
                        Console.Error.WriteLine($"File not found: {icsPath}");
                        continue;
                    }

                    // Load the calendar appointment from the .ics file
                    Appointment appointment = Appointment.Load(icsPath);

                    // Convert the appointment to a MailMessage
                    using (MailMessage mailMessage = appointment.ToMailMessage())
                    {
                        // Add a standardized prefix to the subject
                        const string subjectPrefix = "[Standard] ";
                        mailMessage.Subject = subjectPrefix + (mailMessage.Subject ?? string.Empty);

                        // Determine output .eml file path
                        string emlFileName = Path.GetFileNameWithoutExtension(icsPath) + ".eml";
                        string emlPath = Path.Combine(outputDirectory, emlFileName);

                        // Save the MailMessage as .eml
                        mailMessage.Save(emlPath);

                        Console.WriteLine($"Converted '{icsPath}' to '{emlPath}'.");
                    }
                }
                catch (Exception itemEx)
                {
                    Console.Error.WriteLine($"Error processing '{icsPath}': {itemEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
