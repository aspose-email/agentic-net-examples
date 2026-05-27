using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string icsDirectory = "IcsFiles";
            string outputMsgPath = "CombinedCalendar.msg";

            // Verify the input directory exists
            if (!Directory.Exists(icsDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {icsDirectory}");
                return;
            }

            // Create a new MAPI message that will hold the combined calendar
            using (MapiMessage combinedMessage = new MapiMessage())
            {
                combinedMessage.Subject = "Combined Calendar";

                // Process each .ics file in the directory
                string[] icsFiles = Directory.GetFiles(icsDirectory, "*.ics");
                if (icsFiles.Length == 0)
                {
                    Console.Error.WriteLine("No .ics files found in the specified directory.");
                    return;
                }

                foreach (string icsFilePath in icsFiles)
                {
                    try
                    {
                        // Load the appointment to ensure the file is a valid iCalendar item
                        Appointment appointment = Appointment.Load(icsFilePath);

                        // Read the raw .ics bytes
                        byte[] icsData = File.ReadAllBytes(icsFilePath);

                        // Attach the .ics file to the combined message
                        combinedMessage.Attachments.Add(Path.GetFileName(icsFilePath), icsData);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{icsFilePath}': {ex.Message}");
                    }
                }

                // Save the combined message as a MSG file
                try
                {
                    combinedMessage.Save(outputMsgPath);
                    Console.WriteLine($"Combined calendar saved to: {outputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save combined MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
