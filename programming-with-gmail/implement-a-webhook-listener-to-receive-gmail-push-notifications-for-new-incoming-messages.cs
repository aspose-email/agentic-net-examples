using System;
using System.Net;
using System.Text;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid real network calls.
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Exiting without making network calls.");
                return;
            }

            // Create Gmail client.
            IGmailClient client;
            try
            {
                client = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Use using to ensure disposal of the client.
            using (client)
            {
                // Set up a simple HTTP listener to act as a webhook endpoint.
                string prefix = "http://localhost:5000/notifications/";
                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add(prefix);
                    try
                    {
                        listener.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to start HTTP listener: {ex.Message}");
                        return;
                    }

                    Console.WriteLine($"Listening for Gmail push notifications at {prefix}");
                    while (true)
                    {
                        HttpListenerContext context;
                        try
                        {
                            context = listener.GetContext();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Listener error: {ex.Message}");
                            break;
                        }

                        // Only handle POST requests.
                        if (context.Request.HttpMethod != "POST")
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                            context.Response.Close();
                            continue;
                        }

                        // Read the request body.
                        string requestBody;
                        using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                        {
                            requestBody = reader.ReadToEnd();
                        }

                        // Parse JSON payload (Gmail push notification format).
                        try
                        {
                            using (JsonDocument doc = JsonDocument.Parse(requestBody))
                            {
                                JsonElement root = doc.RootElement;
                                // Example: extract emailAddress and historyId.
                                string emailAddress = root.GetProperty("emailAddress").GetString();
                                string historyId = root.GetProperty("historyId").GetString();

                                Console.WriteLine($"Received notification for {emailAddress}, historyId: {historyId}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to parse notification JSON: {ex.Message}");
                        }

                        // Respond to Gmail to acknowledge receipt.
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        byte[] responseBytes = Encoding.UTF8.GetBytes("OK");
                        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                        context.Response.Close();
                    }

                    listener.Stop();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
