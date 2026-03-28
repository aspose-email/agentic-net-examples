using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize token provider for Outlook (3‑argument overload)
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId: "YOUR_CLIENT_ID",
                clientSecret: "YOUR_CLIENT_SECRET",
                refreshToken: "YOUR_REFRESH_TOKEN");

            // Create Graph client (variable name must be exactly 'client')
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                // Simple example: list messages in the default Inbox folder
                // Folder identifier is obtained via ItemId; using empty string lists the default folder
                foreach (var messageInfo in client.ListMessages(string.Empty))
                {
                    Console.WriteLine($"Message Subject: {messageInfo.Subject}");
                }

                // ----- Convert an .ics file to .msg -----
                string icsPath = "sample.ics";
                if (!File.Exists(icsPath))
{
    try
    {
        string placeholderIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nPRODID:-//Placeholder//EN\r\nBEGIN:VEVENT\r\nUID:placeholder\r\nDTSTAMP:20260101T000000Z\r\nDTSTART:20260101T000000Z\r\nDTEND:20260101T010000Z\r\nSUMMARY:Placeholder\r\nEND:VEVENT\r\nEND:VCALENDAR";
        File.WriteAllText(icsPath, placeholderIcs);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder ICS: {ex.Message}");
        return;
    }
}


                // Load the iCalendar file
                Appointment appointment;
                try
                {
                    appointment = Appointment.Load(icsPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading .ics file: {ex.Message}");
                    return;
                }

                // Convert to MAPI message (MSG)
                MapiMessage msg = appointment.ToMapiMessage();

                string msgPath = "converted.msg";
                try
                {
                    msg.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving .msg file: {ex.Message}");
                    return;
                }

                // ----- Convert the .msg file to .eml -----
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {msgPath}");
                    return;
                }

                MapiMessage loadedMsg;
                try
                {
                    loadedMsg = MapiMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading .msg file: {ex.Message}");
                    return;
                }

                using (loadedMsg)
                {
                    // Convert MAPI message to MailMessage
                    MailMessage mail = loadedMsg.ToMailMessage(new MailConversionOptions());

                    // Save as EML using proper SaveOptions
                    string emlPath = "converted.eml";
                    try
                    {
                        mail.Save(emlPath, SaveOptions.DefaultEml);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving .eml file: {ex.Message}");
                        return;
                    }
                }

                Console.WriteLine("Conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
