using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Microsoft Graph authentication placeholders
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string tenantId = "YOUR_TENANT_ID";

            // Guard placeholders
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") || tenantId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace placeholder credentials with actual values.");
                // Continue without Graph operations; proceed with local file conversion.
            }

            // Assume .ics file is already present locally.
            string icsLocalPath = "event.ics";

            if (!File.Exists(icsLocalPath))
            {
                Console.Error.WriteLine($".ics file not found at '{icsLocalPath}'.");
                return;
            }

            // Convert .ics to .msg
            Appointment appointment = Appointment.Load(icsLocalPath);
            MapiMessage mapMsg = appointment.ToMapiMessage();
            string msgPath = "event.msg";
            using (MailMessage mailMsg = mapMsg.ToMailMessage(new MailConversionOptions()))
            {
                mailMsg.Save(msgPath);
            }
            Console.WriteLine($"Converted .ics to MSG: {msgPath}");

            // Convert .msg to .eml
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"MSG file not found at '{msgPath}'.");
                return;
            }

            MapiMessage loadedMapi = MapiMessage.Load(msgPath);
            string emlPath = "event.eml";
            using (MailMessage loadedMail = loadedMapi.ToMailMessage(new MailConversionOptions()))
            {
                loadedMail.Save(emlPath);
            }
            Console.WriteLine($"Converted MSG to EML: {emlPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
