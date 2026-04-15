using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Organizer to filter appointments
            string organizerEmail = "organizer@example.com";

            // Output CSV file path
            string csvPath = "appointments.csv";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || organizerEmail.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            string csvDirectory = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                Directory.CreateDirectory(csvDirectory);
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Retrieve all appointments from the default calendar folder
                Appointment[] allAppointments = client.ListAppointments();

                // Filter appointments by organizer
                List<Appointment> filteredAppointments = new List<Appointment>();
                foreach (Appointment appointment in allAppointments)
                {
                    if (appointment.Organizer != null &&
                        string.Equals(appointment.Organizer.Address, organizerEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredAppointments.Add(appointment);
                    }
                }

                // Write filtered appointments to CSV
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    // CSV header
                    writer.WriteLine("Subject,StartDate,EndDate,Location");

                    foreach (Appointment appointment in filteredAppointments)
                    {
                        string subject = appointment.Summary != null ? appointment.Summary.Replace("\"", "\"\"") : "";
                        string location = appointment.Location != null ? appointment.Location.Replace("\"", "\"\"") : "";
                        writer.WriteLine($"\"{subject}\",\"{appointment.StartDate:O}\",\"{appointment.EndDate:O}\",\"{location}\"");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
