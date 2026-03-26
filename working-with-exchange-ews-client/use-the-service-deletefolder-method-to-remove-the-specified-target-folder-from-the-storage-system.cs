using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

public class Program
{
    public static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        // Top‑level exception guard
        try
        {
            // Prepare credentials (replace with real values)
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Client connection safety guard
            try
            {
                // Create EWS client via factory; returns an IEWSClient instance
                using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
                {
                    // Target folder URI to delete (replace with actual folder URI)
                    string folderUri = "Inbox/TargetFolder";

                    // Attempt to delete the folder
                    try
                    {
                        client.DeleteFolder(folderUri);
                        Console.WriteLine("Folder deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error deleting folder: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error creating or using the EWS client: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}