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
            // File paths
            string icsPath = "sample.ics";
            string msgPath = "output.msg";
            string emlPath = "output.eml";

            // Verify input .ics file exists
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {icsPath}");
                return;
            }

            // Load appointment from .ics
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

            // Convert to MapiMessage and save as .msg
            using (MapiMessage msg = appointment.ToMapiMessage())
            {
                try
                {
                    msg.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving .msg file: {ex.Message}");
                    return;
                }
            }

            // Verify .msg file exists before further processing
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load MapiMessage from .msg
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

            // Convert to MailMessage and save as .eml
            using (MailMessage mailMessage = loadedMsg.ToMailMessage(new MailConversionOptions()))
            {
                try
                {
                    mailMessage.Save(emlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving .eml file: {ex.Message}");
                    return;
                }
            }

            // Initialize Microsoft Graph client (dummy credentials)
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Placeholder for Graph operations (e.g., uploading the MSG)
                Console.WriteLine("Graph client initialized successfully.");
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
