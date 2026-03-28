using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients;

namespace Pop3EventSample
{
    class Pop3Monitor : IDisposable
    {
        private readonly Pop3Client _client;
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        private int _previousMessageCount = -1;
        private Task _monitoringTask;

        public event EventHandler MailboxChanged;

        public Pop3Monitor(Pop3Client client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public void Start()
        {
            _monitoringTask = Task.Run(async () => await MonitorAsync(_cancellationSource.Token));
        }

        public void Stop()
        {
            _cancellationSource.Cancel();
            try
            {
                _monitoringTask?.Wait();
            }
            catch (AggregateException) { }
        }

        private async Task MonitorAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    int currentCount = await _client.GetMessageCountAsync(cancellationToken);
                    if (_previousMessageCount != -1 && currentCount != _previousMessageCount)
                    {
                        MailboxChanged?.Invoke(this, EventArgs.Empty);
                    }
                    _previousMessageCount = currentCount;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while checking mailbox: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }

        public void Dispose()
        {
            Stop();
            _cancellationSource.Dispose();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Configure POP3 client credentials
                string host = "pop3.example.com";
                string username = "user@example.com";
                string password = "password";

                using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                    }
                    catch (Exception connEx)
                    {
                        Console.Error.WriteLine($"Connection error: {connEx.Message}");
                        return;
                    }

                    using (Pop3Monitor monitor = new Pop3Monitor(client))
                    {
                        monitor.MailboxChanged += (sender, e) =>
                        {
                            Console.WriteLine("Mailbox change detected.");
                        };

                        monitor.Start();

                        Console.WriteLine("Monitoring mailbox changes. Press Enter to stop.");
                        Console.ReadLine();

                        monitor.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
