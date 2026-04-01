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
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a Calendar folder manually
                        pst.RootFolder.AddSubFolder("Calendar");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Attempt to retrieve the Calendar folder
                FolderInfo calendarFolder = null;
                try
                {
                    calendarFolder = pst.RootFolder.GetSubFolder("Calendar");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing Calendar folder: {ex.Message}");
                    return;
                }

                if (calendarFolder == null)
                {
                    Console.Error.WriteLine("Calendar folder not found in the PST.");
                    return;
                }

                // Enumerate calendar items (appointments) and display basic info
                foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                {
                    try
                    {
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Check if the message is a calendar item
                            if (mapiMessage.SupportedType == MapiItemType.Calendar)
                            {
                                MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();
                                Console.WriteLine($"Subject: {calendar.Subject}");
                                Console.WriteLine($"Start: {calendar.StartDate}");
                                Console.WriteLine($"End:   {calendar.EndDate}");
                                Console.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message ID {messageInfo.EntryId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
