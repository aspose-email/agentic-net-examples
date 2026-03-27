using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        try
        {
            // URL that initiates the authorization flow
            string authorizationUrl = "https://example.com/authorize";

            // Open the default web browser to let the user complete the flow
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = authorizationUrl,
                UseShellExecute = true
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open browser: {ex.Message}");
                return;
            }

            // Prompt the user to paste the resulting authorization code
            Console.WriteLine("After completing the authorization, please enter the authorization code:");
            string authorizationCode = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                Console.Error.WriteLine("No authorization code was entered.");
                return;
            }

            Console.WriteLine($"Authorization code received: {authorizationCode}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
