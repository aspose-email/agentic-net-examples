using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the folder containing MSG files and the output CSV path
                string inputFolderPath = "Calendars";
                string outputCsvPath = "CalendarReport.csv";

                // Verify input folder exists
                if (!Directory.Exists(inputFolderPath))
                {
                    Console.Error.WriteLine($"Input folder does not exist: {inputFolderPath}");
                    return;
                }

                // Prepare a list to hold file name and start date
                List<(string FileName, DateTime StartDate)> calendarEntries = new List<(string, DateTime)>();

                // Get all .msg files in the folder
                string[] msgFiles;
                try
                {
                    msgFiles = Directory.GetFiles(inputFolderPath, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                    return;
                }

                foreach (string msgFilePath in msgFiles)
                {
                    // Guard against missing file (should not happen after GetFiles, but safe)
                    if (!File.Exists(msgFilePath))
                    {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    if (string.IsNullOrEmpty(placeholderCalendar.Subject))
                    {
                        placeholderCalendar.Subject = "Placeholder Summary";
                    }
                    if (string.IsNullOrEmpty(placeholderCalendar.Body))
                    {
                        placeholderCalendar.Body = "Placeholder Description";
                    }
                    placeholderCalendar.Save(msgFilePath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"File not found: {msgFilePath}");
                        continue;
                    }

                    // Load the MSG file
                    MapiMessage msg;
                    try
                    {
                        msg = MapiMessage.Load(msgFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load MSG file '{msgFilePath}': {ex.Message}");
                        continue;
                    }

                    // Process only calendar items
                    if (msg.SupportedType == MapiItemType.Calendar)
                    {
                        MapiCalendar calendar = msg.ToMapiMessageItem() as MapiCalendar;
                        if (calendar != null)
                        {
                            DateTime startDate = calendar.StartDate;
                            calendarEntries.Add((Path.GetFileName(msgFilePath), startDate));
                        }
                        else
                        {
                            Console.Error.WriteLine($"Unable to convert message to MapiCalendar: {msgFilePath}");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"MSG file is not a calendar item: {msgFilePath}");
                    }

                    // Dispose the message
                    msg.Dispose();
                }

                // Sort entries chronologically by start date
                calendarEntries.Sort((x, y) => DateTime.Compare(x.StartDate, y.StartDate));

                // Ensure the directory for the output CSV exists
                string outputDirectory = Path.GetDirectoryName(outputCsvPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {ex.Message}");
                        return;
                    }
                }

                // Write the CSV report
                try
                {
                    using (StreamWriter writer = new StreamWriter(outputCsvPath, false))
                    {
                        // Header
                        writer.WriteLine("FileName,StartDate");

                        // Data rows
                        foreach ((string FileName, DateTime StartDate) entry in calendarEntries)
                        {
                            writer.WriteLine($"{entry.FileName},{entry.StartDate:O}");
                        }
                    }

                    Console.WriteLine($"CSV report generated at: {outputCsvPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write CSV file '{outputCsvPath}': {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
