using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the input .ics file, the temporary .msg file and the PST file.
            string icsPath = "sample.ics";
            string msgPath = "appointment.msg";
            string pstPath = "sample.pst";

            // Verify that the .ics file exists before proceeding.
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {icsPath}");
                return;
            }

            // Load the calendar appointment from the .ics file.
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

            // Convert the appointment to a MAPI message.
            MapiMessage mapiMessage = appointment.ToMapiMessage();

            // Save the MAPI message as a .msg file (used later as an attachment).
            try
            {
                mapiMessage.Save(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving .msg file: {ex.Message}");
                return;
            }

            // Ensure the PST file exists; create it if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file and add the message to a custom folder.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the standard Inbox folder.
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                if (inboxFolder == null)
                {
                    Console.Error.WriteLine("Error: Unable to retrieve the Inbox folder from PST.");
                    return;
                }

                // Create a subfolder named "MyFolder" under the Inbox.
                FolderInfo myFolder = inboxFolder.AddSubFolder("MyFolder");

                // Add the MAPI message to the newly created folder.
                myFolder.AddMessage(mapiMessage);
            }

            // Send an email with the .msg file attached using SMTP.
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587, "user@example.com", "password"))
                {
                    smtpClient.SecurityOptions = SecurityOptions.Auto;

                    MailMessage email = new MailMessage(
                        "user@example.com",
                        "recipient@example.com",
                        "Appointment Export",
                        "Please find the appointment attached as a .msg file."
                    );

                    // Attach the .msg file.
                    email.Attachments.Add(new Attachment(msgPath));

                    smtpClient.Send(email);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP error: {ex.Message}");
                // Continue without rethrowing to keep the application stable.
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
