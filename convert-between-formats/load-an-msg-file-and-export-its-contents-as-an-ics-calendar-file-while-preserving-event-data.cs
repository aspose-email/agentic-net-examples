using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "calendar.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            string icsPath = "calendar.ics";
            string outputDir = Path.GetDirectoryName(icsPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    if (msg.SupportedType != MapiItemType.Calendar)
                    {
                        Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                        return;
                    }

                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();
                    using (calendar)
                    {
                        calendar.Save(icsPath, MapiCalendarSaveOptions.DefaultIcs);
                        Console.WriteLine($"Calendar exported to {icsPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}