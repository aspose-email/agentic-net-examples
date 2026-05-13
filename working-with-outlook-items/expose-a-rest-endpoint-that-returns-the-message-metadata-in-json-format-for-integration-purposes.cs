using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string exchangeUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (exchangeUri.Contains("example.com") || username == "user")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external call.");
                return;
            }

            // Start a simple HTTP listener
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/metadata/");
            try
            {
                listener.Start();
                Console.WriteLine("Listening on http://localhost:5000/metadata/ ...");

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;

                    // Expect a query parameter "folder", default to "Inbox"
                    string folder = request.QueryString["folder"] ?? "Inbox";

                    // Prepare a list to hold message metadata
                    List<object> metadataList = new List<object>();

                    // Connect to Exchange and fetch messages
                    try
                    {
                        using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                        {
                            // List messages in the specified folder
                            ExchangeMessageInfoCollection messages = client.ListMessages(folder);
                            foreach (var msgInfo in messages)
                            {
                                var meta = new
                                {
                                    Subject = msgInfo.Subject,
                                    From = msgInfo.From?.ToString(),
                                    To = msgInfo.To?.ToString(),
                                    Date = msgInfo.InternalDate,
                                    Size = msgInfo.Size
                                };
                                metadataList.Add(meta);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error accessing Exchange: {ex.Message}");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        byte[] errorBytes = Encoding.UTF8.GetBytes($"{{\"error\":\"{ex.Message}\"}}");
                        response.OutputStream.Write(errorBytes, 0, errorBytes.Length);
                        response.OutputStream.Close();
                        continue;
                    }

                    // Serialize metadata to JSON
                    string json = JsonSerializer.Serialize(metadataList, new JsonSerializerOptions { WriteIndented = true });
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    response.ContentType = "application/json";
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
            }
            finally
            {
                listener.Stop();
                listener.Close();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
