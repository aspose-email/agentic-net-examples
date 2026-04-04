using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the predefined Appointments folder
                    FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);

                    // Create a new calendar item
                    MapiCalendar newCalendar = new MapiCalendar(
                        "Team Sync",
                        "Conference Room",
                        "Weekly team synchronization meeting.",
                        DateTime.Now.AddDays(1).AddHours(9),
                        DateTime.Now.AddDays(1).AddHours(10));

                    // Add the calendar to the folder and obtain its entry ID
                    string entryId = calendarFolder.AddMapiMessageItem(newCalendar);
                    Console.WriteLine($"Calendar added with EntryId: {entryId}");

                    // Extract the message to work with it
                    MapiMessage extractedMessage = pst.ExtractMessage(entryId);
                    if (extractedMessage.SupportedType == MapiItemType.Calendar)
                    {
                        // Convert to MapiCalendar for manipulation
                        MapiCalendar calendar = (MapiCalendar)extractedMessage.ToMapiMessageItem();

                        // Display original subject
                        Console.WriteLine($"Original Subject: {calendar.Subject}");

                        // Modify the subject
                        calendar.Subject = "Updated Team Sync";

                        // Update the calendar item in the PST
                        calendarFolder.UpdateMessage(entryId, calendar);
                        Console.WriteLine("Calendar subject updated.");

                        // Verify update by re-extracting
                        MapiMessage verifyMessage = pst.ExtractMessage(entryId);
                        MapiCalendar verifyCalendar = (MapiCalendar)verifyMessage.ToMapiMessageItem();
                        Console.WriteLine($"Verified Subject: {verifyCalendar.Subject}");
                    }
                    else
                    {
                        Console.WriteLine("The added item is not a calendar.");
                    }

                    // Delete the calendar item
                    pst.DeleteItem(entryId);
                    Console.WriteLine("Calendar item deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"PST operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
