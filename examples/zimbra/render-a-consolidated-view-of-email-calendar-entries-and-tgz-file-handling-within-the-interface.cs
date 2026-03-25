using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;
using Aspose.Email.Calendar;
using Aspose.Email.Tools.Search;

namespace AsposeEmailZimbraExample
{
    class Program
    {
        static void Main(string[] args)
        {
        List<MailMessage> messages = new List<MailMessage>();

            try
            {
                // Paths to sample files
                string tgzFilePath = "sample.tgz";
                string emlFilePath = "sample.eml";
                string icsFilePath = "sample.ics";

                // Process TGZ archive (Zimbra)
                if (File.Exists(tgzFilePath))
                {
                    using (TgzReader tgzReader = new TgzReader(tgzFilePath))
                    {
                        Console.WriteLine("Reading messages from TGZ archive:");
                        while (tgzReader.ReadNextMessage())
                        {
                            // CurrentMessage returns a MailMessage instance
                            using (MailMessage message = tgzReader.CurrentMessage)
                            {
                                if (message != null)
                                {
                                    Console.WriteLine("Subject: " + message.Subject);
                                    Console.WriteLine("From: " + message.From);
                                    Console.WriteLine("Date: " + message.Date);
                                    Console.WriteLine(new string('-', 40));
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.Error.WriteLine("TGZ file not found: " + tgzFilePath);
                }

                // Load and display a single EML email
                if (File.Exists(emlFilePath))
                {
                    EmlLoadOptions emlOptions = new EmlLoadOptions();
                    using (MailMessage email = MailMessage.Load(emlFilePath, emlOptions))
                    {
                        Console.WriteLine("Loaded EML message:");
                        Console.WriteLine("Subject: " + email.Subject);
                        Console.WriteLine("From: " + email.From);
                        Console.WriteLine("To: " + email.To);
                        Console.WriteLine("Date: " + email.Date);
                        Console.WriteLine(new string('=', 40));
                    }
                }
                else
                {
                    Console.Error.WriteLine("EML file not found: " + emlFilePath);
                }

                // Load and display calendar entries from an iCalendar file
                if (File.Exists(icsFilePath))
                {
                    AppointmentLoadOptions calOptions = new AppointmentLoadOptions();
                    Appointment appointment = Appointment.Load(icsFilePath, calOptions);
                    Console.WriteLine("Loaded Calendar Appointment:");
                    Console.WriteLine("Summary: " + appointment.Summary);
                    Console.WriteLine("Location: " + appointment.Location);
                    Console.WriteLine("Start: " + appointment.StartDate);
                    Console.WriteLine("End: " + appointment.EndDate);
                    Console.WriteLine("Organizer: " + appointment.Organizer);
                    Console.WriteLine(new string('~', 40));
                }
                else
                {
                    Console.Error.WriteLine("iCalendar file not found: " + icsFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}