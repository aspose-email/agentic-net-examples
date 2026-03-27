using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for input and output files
                string icsPath = "sample.ics";
                string msgPath = "output.msg";
                string emlPath = "output.eml";

                // Guard input file existence
                if (!File.Exists(icsPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {icsPath}");
                    return;
                }

                // Load the iCalendar file
                Appointment appointment = Appointment.Load(icsPath);

                // Convert the appointment to a MAPI message (MSG)
                MapiMessage msg = appointment.ToMapiMessage();

                // Save the MSG file
                msg.Save(msgPath);

                // Load the saved MSG file
                MapiMessage loadedMsg = MapiMessage.Load(msgPath);

                // Convert the MAPI message to a MailMessage (EML)
                MailMessage mail = loadedMsg.ToMailMessage(new MailConversionOptions());

                // Save the EML file
                mail.Save(emlPath);

                // Initialize Graph client (placeholder credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Example operation: send the MSG as MIME using Graph client
                    // This demonstrates usage of the Graph client as required
                    graphClient.SendAsMime(msg);
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}