using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string pstPath = "sample.pst";
            string icsPath = "sample.ics";
            string tgzPath = "sample.tgz";
            string outputDir = "output";

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // ---------- PST handling ----------
            if (!File.Exists(pstPath))
            {
                // Create an empty PST file if missing
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folder.DisplayName}, Items: {folder.ContentCount}, Unread: {folder.ContentUnreadCount}");

                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        Console.WriteLine($"  Message Subject: {msgInfo.Subject}");

                        using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                        {
                            // Save each message as .msg in the output directory
                            string safeSubject = string.IsNullOrWhiteSpace(msgInfo.Subject) ? "Untitled" : msgInfo.Subject;
                            string msgFileName = Path.Combine(outputDir, $"{safeSubject}.msg");
                            mapiMsg.Save(msgFileName);
                        }
                    }
                }
            }

            // ---------- Calendar (ICS) handling ----------
            if (!File.Exists(icsPath))
            {
                // Create a minimal placeholder iCalendar file
                File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
            }

            Appointment appointment = Appointment.Load(icsPath);
            Console.WriteLine($"Calendar Summary: {appointment.Summary}");
            Console.WriteLine($"Start: {appointment.StartDate}, End: {appointment.EndDate}");

            // ---------- TGZ handling ----------
            if (File.Exists(tgzPath))
            {
                using (TgzReader tgz = new TgzReader(tgzPath))
                {
                    Console.WriteLine($"TGZ contains {tgz.GetTotalItemsCount()} items.");
                    // Export all messages and directory structure to the output folder
                    tgz.ExportTo(outputDir);
                }
            }
            else
            {
                Console.WriteLine("TGZ file not found; skipping TGZ processing.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
