using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder network settings
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder service URL detected. Skipping network operation.");
                return;
            }

            // Path to the existing MSG file containing the calendar event
            string msgFilePath = "event.msg";

            // Ensure the MSG file exists; if not, create a minimal placeholder
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholderMsg = new MapiMessage())
                    {
                        placeholderMsg.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage loadedMessage;
            try
            {
                loadedMessage = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Verify that the MSG contains a calendar item
            if (loadedMessage.SupportedType != MapiItemType.Calendar)
            {
                // Create a minimal placeholder ICS file
                string placeholderIcsPath = "placeholder.ics";
                try
                {
                    string icsContent = "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n";
                    File.WriteAllText(placeholderIcsPath, icsContent);
                    Console.WriteLine($"Input MSG is not a calendar. Placeholder ICS created at '{placeholderIcsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder ICS file: {ex.Message}");
                }
                return;
            }

            // Convert to MapiCalendar
            MapiCalendar calendarItem = (MapiCalendar)loadedMessage.ToMapiMessageItem();

            // Update the location property
            calendarItem.Location = "New Conference Room";

            // Update the event on the Exchange server using EWS
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    client.UpdateAppointment(calendarItem);
                }
                Console.WriteLine("Calendar event location updated successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to update calendar event: {ex.Message}");
                return;
            }

            // Optionally, save the modified calendar back to a MSG file
            string updatedMsgPath = "event_updated.msg";
            try
            {
                using (MapiMessage updatedMessage = calendarItem.GetUnderlyingMessage())
                {
                    updatedMessage.Save(updatedMsgPath);
                }
                Console.WriteLine($"Updated MSG saved to '{updatedMsgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save updated MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
