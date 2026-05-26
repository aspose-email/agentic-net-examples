using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string xmlPath = "smtp_credentials.xml";

            if (!File.Exists(xmlPath))
            {
                Console.Error.WriteLine($"Credentials file not found: {xmlPath}");
                return;
            }

            XDocument doc;
            try
            {
                doc = XDocument.Load(xmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load XML: {ex.Message}");
                return;
            }

            XElement root = doc.Root;
            if (root == null)
            {
                Console.Error.WriteLine("Invalid XML format.");
                return;
            }

            string host = (string)root.Element("Host") ?? "";
            string portString = (string)root.Element("Port") ?? "25";
            int port = int.TryParse(portString, out int parsedPort) ? parsedPort : 25;
            string username = (string)root.Element("Username") ?? "";
            string password = (string)root.Element("Password") ?? "";

            NetworkCredential credential = new NetworkCredential(username, password);

            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = credential.UserName;
                    client.Password = credential.Password;
                    client.SecurityOptions = SecurityOptions.Auto;

                    // Client is ready for sending emails.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
