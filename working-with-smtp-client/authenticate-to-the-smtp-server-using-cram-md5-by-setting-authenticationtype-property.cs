using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping connection.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Set authentication mechanism to CRAM‑MD5 using reflection (property may differ across versions)
                    Type authEnumType = Type.GetType("Aspose.Email.Clients.Smtp.SmtpAuthenticationType, Aspose.Email");
                    if (authEnumType != null)
                    {
                        object cramValue = Enum.Parse(authEnumType, "CramMd5", ignoreCase: true);
                        var authProp = client.GetType().GetProperty("AuthenticationType");
                        if (authProp != null && authProp.CanWrite)
                        {
                            authProp.SetValue(client, cramValue);
                        }
                    }

                    // Validate the credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Authentication succeeded." : "Authentication failed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
