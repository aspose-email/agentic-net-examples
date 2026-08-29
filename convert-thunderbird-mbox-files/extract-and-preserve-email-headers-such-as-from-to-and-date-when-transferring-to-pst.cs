using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string msgPath = "message.msg";
            const string pstPath = "output.pst";

            // Verify the MSG file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure the PST file exists; create a minimal PST if it does not
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {createEx.Message}");
                    return;
                }
            }

            // Load the MSG file as a MAPI message
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage to read headers
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    Console.WriteLine($"From: {mailMessage.From}");
                    Console.WriteLine($"To: {mailMessage.To}");
                    Console.WriteLine($"Date: {mailMessage.Date}");
                }

                // Open the PST and add the original MAPI message, preserving all headers
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    pst.RootFolder.AddMessage(mapiMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
