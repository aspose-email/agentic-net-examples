using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "InputIcs";
            string outputDirectory = "OutputEml";

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
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Process each .ics file in the input directory
            string[] icsFiles = Directory.GetFiles(inputDirectory, "*.ics");
            foreach (string icsPath in icsFiles)
            {
                try
                {
                    // Load the iCalendar file as an Appointment
                    Appointment appointment = Appointment.Load(icsPath);

                    // Convert to a MailMessage (EML)
                    using (MailMessage mailMessage = appointment.ToMailMessage())
                    {
                        // Standardize subject with a prefix
                        string originalSubject = appointment.Summary ?? string.Empty;
                        mailMessage.Subject = $"[Converted] {originalSubject}";

                        // Build output file path using the original file name (without extension)
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(icsPath);
                        string emlPath = Path.Combine(outputDirectory, $"{fileNameWithoutExt}.eml");

                        // Save the MailMessage as .eml
                        mailMessage.Save(emlPath);
                        Console.WriteLine($"Converted '{icsPath}' to '{emlPath}'.");
                    }
                }
                catch (Exception exFile)
                {
                    Console.Error.WriteLine($"Error processing file '{icsPath}': {exFile.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
