using System;
using System.Reflection;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailColorsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with dummy credentials.
                // Replace with real values when running in a real environment.
                using (IGmailClient gmailClient = GmailClient.GetInstance(
                    "accessToken",
                    "user@example.com"))
                {
                    // Retrieve color information.
                    // GetColors returns a ColorsInfo object containing Gmail UI color settings.
                    // The exact members of ColorsInfo are not documented here,
                    // so we enumerate its public properties via reflection.
                    object colorsInfo = gmailClient.GetColors();

                    if (colorsInfo == null)
                    {
                        Console.Error.WriteLine("Failed to retrieve color information.");
                        return;
                    }

                    Type colorsType = colorsInfo.GetType();
                    PropertyInfo[] properties = colorsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    Console.WriteLine("Gmail Color Attributes:");
                    foreach (PropertyInfo prop in properties)
                    {
                        try
                        {
                            object value = prop.GetValue(colorsInfo);
                            Console.WriteLine($"{prop.Name}: {value}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Unable to read property '{prop.Name}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
