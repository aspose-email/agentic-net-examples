using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        try
        {
            // URL that initiates the authorization flow
            string authorizationUrl = "https://example.com/oauth2/authorize";

            // Open the default browser to let the user complete the required actions
            ProcessStartInfo startInfo = new ProcessStartInfo(authorizationUrl)
            {
                UseShellExecute = true
            };
            Process.Start(startInfo);

            // Prompt the user to paste the resulting authorization code
            Console.Write("Enter the authorization code: ");
            string authorizationCode = Console.ReadLine();

            // Output the retrieved code (or handle it as needed)
            Console.WriteLine("Authorization code received: " + authorizationCode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            return;
        }
    }
}
