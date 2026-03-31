using Aspose.Email.Clients.Exchange;
using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when using placeholder credentials
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping client operations.");
                return;
            }

            // Create the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Path for trace output
                    string logPath = "ews_trace.log";

                    // Ensure the directory for the log file exists
                    try
                    {
                        string directory = Path.GetDirectoryName(Path.GetFullPath(logPath));
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to prepare log directory: {dirEx.Message}");
                        return;
                    }

                    // Configure TraceListener
                    try
                    {
                        using (TextWriterTraceListener listener = new TextWriterTraceListener(logPath))
                        {
                            Trace.Listeners.Clear();
                            Trace.Listeners.Add(listener);
                            Trace.AutoFlush = true;

                            // Example operation to generate trace output
                            ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                            Console.WriteLine($"Mailbox URI: {mailboxInfo.MailboxUri}");
                        }
                    }
                    catch (Exception traceEx)
                    {
                        Console.Error.WriteLine($"Error configuring trace listener: {traceEx.Message}");
                    }
                    finally
                    {
                        // Clean up trace listeners
                        Trace.Listeners.Clear();
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to create or use EWS client: {clientEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
