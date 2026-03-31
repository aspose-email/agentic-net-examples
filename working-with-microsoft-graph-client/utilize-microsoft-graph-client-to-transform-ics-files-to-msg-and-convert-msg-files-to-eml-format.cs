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
            // Placeholder credentials for Microsoft Graph
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";
            string tenantId = "your_tenant_id";

            // Guard against placeholder credentials – skip external calls
            if (clientId.StartsWith("your_") || clientSecret.StartsWith("your_") || refreshToken.StartsWith("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph client operations.");
                return;
            }

            // Create token provider (must be Aspose.Email.Clients.ITokenProvider, not the static class itself)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client (will be disposed automatically)
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Paths for conversion
                string icsInputPath = "input.ics";
                string msgOutputPath = "output.msg";
                
            string outputDir = Path.GetDirectoryName(msgOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string msgInputPath = "input.msg";
                string emlOutputPath = "output.eml";

                // ---------- Convert ICS to MSG ----------
                try
                {
                    if (!File.Exists(icsInputPath))
                    {
                        // Create a minimal placeholder .ics file
                        File.WriteAllText(icsInputPath,
@"BEGIN:VCALENDAR
VERSION:2.0
BEGIN:VEVENT
DTSTART:20240101T090000Z
DTEND:20240101T100000Z
SUMMARY:Sample Event
END:VEVENT
END:VCALENDAR");
                    }

                    // Load the calendar appointment
                    Appointment appointment = Appointment.Load(icsInputPath);

                    // Convert to MAPI message (MSG)
                    MapiMessage msgFromIcs = appointment.ToMapiMessage();

                    // Save MSG file
                    msgFromIcs.Save(msgOutputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error converting ICS to MSG: {ex.Message}");
                    return;
                }

                // ---------- Convert MSG to EML ----------
                try
                {
                    if (!File.Exists(msgInputPath))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgInputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        // Create a minimal placeholder MSG file
                        using (MapiMessage placeholderMsg = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                        {
                            placeholderMsg.Save(msgInputPath);
                        }
                    }

                    // Load the MSG file
                    using (MapiMessage msg = MapiMessage.Load(msgInputPath))
                    {
                        // Convert to MailMessage
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                        {
                            // Save as EML using proper SaveOptions
                            mail.Save(emlOutputPath, SaveOptions.DefaultEml);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error converting MSG to EML: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
