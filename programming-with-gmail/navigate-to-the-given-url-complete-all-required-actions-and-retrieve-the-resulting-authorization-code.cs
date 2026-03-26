using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual client ID and desired scopes
            string clientId = "YOUR_CLIENT_ID";
            string redirectUri = "urn:ietf:wg:oauth:2.0:oob";
            string scope = "https://mail.google.com/";
            string authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope={Uri.EscapeDataString(scope)}&access_type=offline";

            Console.WriteLine("Opening the default browser for Google OAuth consent...");
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = authUrl;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }

            Console.Write("After granting access, paste the authorization code here: ");
            string authorizationCode = Console.ReadLine();

            Console.WriteLine($"Authorization code received: {authorizationCode}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}