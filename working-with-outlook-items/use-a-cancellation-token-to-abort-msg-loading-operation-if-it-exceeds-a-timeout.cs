using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "from@example.com";
                        placeholder.To.Add("to@example.com");
                        placeholder.Subject = "Placeholder MSG";
                        placeholder.Body = "This is a placeholder MSG file.";
                        placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Set up a cancellation token that triggers after a timeout (e.g., 5 seconds).
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromSeconds(5));

                Task<MailMessage> loadTask = Task.Run(() =>
                {
                    MsgLoadOptions loadOptions = new MsgLoadOptions();
                    // Optional: set loadOptions.Timeout if desired (in milliseconds).
                    // loadOptions.Timeout = 3000; // 3 seconds
                    return MailMessage.Load(msgPath, loadOptions);
                }, cts.Token);

                try
                {
                    using (MailMessage message = loadTask.GetAwaiter().GetResult())
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.Error.WriteLine("Loading the MSG file was canceled due to timeout.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
