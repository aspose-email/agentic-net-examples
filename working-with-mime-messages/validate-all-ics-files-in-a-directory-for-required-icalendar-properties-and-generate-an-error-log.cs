using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar;

namespace ValidateIcsFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Determine the directory to scan; use first argument or current directory.
                string directoryPath;
                if (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                {
                    directoryPath = args[0];
                }
                else
                {
                    directoryPath = Directory.GetCurrentDirectory();
                }

                // Verify that the directory exists.
                if (!Directory.Exists(directoryPath))
                {
                    Console.Error.WriteLine($"Directory does not exist: {directoryPath}");
                    return;
                }

                // Prepare a list to collect validation error messages.
                List<string> errorLog = new List<string>();

                // Get all .ics files in the directory (non-recursive).
                string[] icsFiles;
                try
                {
                    icsFiles = Directory.GetFiles(directoryPath, "*.ics");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to enumerate .ics files: {ex.Message}");
                    return;
                }

                if (icsFiles == null || icsFiles.Length == 0)
                {
                    Console.WriteLine("No .ics files found to validate.");
                    return;
                }

                // Process each .ics file.
                foreach (string icsFilePath in icsFiles)
                {
                    // Ensure the file exists before attempting to load.
                    if (!File.Exists(icsFilePath))
                    {
                        errorLog.Add($"File not found: {icsFilePath}");
                        continue;
                    }

                    Appointment appointment = null;
                    try
                    {
                        // Load the iCalendar file into an Appointment object.
                        appointment = Appointment.Load(icsFilePath);
                    }
                    catch (Exception ex)
                    {
                        errorLog.Add($"Failed to load '{icsFilePath}': {ex.Message}");
                        continue;
                    }

                    // Validate required properties.
                    // Required: Summary, StartDate, EndDate, Organizer.
                    if (string.IsNullOrEmpty(appointment.Summary))
                    {
                        errorLog.Add($"'{icsFilePath}': Missing Summary.");
                    }

                    if (appointment.StartDate == DateTime.MinValue)
                    {
                        errorLog.Add($"'{icsFilePath}': Missing or invalid StartDate.");
                    }

                    if (appointment.EndDate == DateTime.MinValue)
                    {
                        errorLog.Add($"'{icsFilePath}': Missing or invalid EndDate.");
                    }

                    if (appointment.Organizer == null || string.IsNullOrEmpty(appointment.Organizer.Address))
                    {
                        errorLog.Add($"'{icsFilePath}': Missing Organizer.");
                    }
                }

                // Write the error log to a file in the same directory.
                string logFilePath = Path.Combine(directoryPath, "validation_errors.log");
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, false))
                    {
                        if (errorLog.Count == 0)
                        {
                            writer.WriteLine("No validation errors found.");
                            Console.WriteLine("All .ics files passed validation.");
                        }
                        else
                        {
                            foreach (string logEntry in errorLog)
                            {
                                writer.WriteLine(logEntry);
                            }
                            Console.WriteLine($"Validation completed with {errorLog.Count} error(s). See log at: {logFilePath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write log file '{logFilePath}': {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
