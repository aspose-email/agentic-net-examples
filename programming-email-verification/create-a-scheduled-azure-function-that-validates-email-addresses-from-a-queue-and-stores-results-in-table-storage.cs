using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationConsoleApp
{
    // Dummy Azure Function attributes
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FunctionNameAttribute : Attribute
    {
        public FunctionNameAttribute(string name) { }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueueTriggerAttribute : Attribute
    {
        public QueueTriggerAttribute(string queueName) { }
        public string Connection { get; set; }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute(string tableName) { }
        public string Connection { get; set; }
    }

    // Dummy logger interface
    public interface ILogger
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(Exception exception, string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void LogInformation(string message) => Console.WriteLine($"[Info] {message}");
        public void LogWarning(string message) => Console.WriteLine($"[Warn] {message}");
        public void LogError(Exception exception, string message) => Console.WriteLine($"[Error] {message} - {exception}");
    }

    // Dummy Table storage types
    public struct ETag { }

    public interface ITableEntity
    {
        string PartitionKey { get; set; }
        string RowKey { get; set; }
        DateTimeOffset? Timestamp { get; set; }
        ETag ETag { get; set; }
    }

    public class TableClient
    {
        private readonly List<ITableEntity> _store = new List<ITableEntity>();

        public Task CreateIfNotExistsAsync() => Task.CompletedTask;

        public Task UpsertEntityAsync(ITableEntity entity)
        {
            _store.Add(entity);
            return Task.CompletedTask;
        }

        // For demonstration purposes
        public IEnumerable<ITableEntity> GetAllEntities() => _store;
    }

    public class EmailValidationEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Email { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    public static class EmailValidationFunction
    {
        [FunctionName("ValidateEmailFromQueue")]
        public static async Task Run(
            [QueueTrigger("emailqueue", Connection = "AzureWebJobsStorage")] string email,
            ILogger log,
            [Table("EmailValidationResults", Connection = "AzureWebJobsStorage")] TableClient tableClient)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    log.LogWarning("Received empty email address from queue.");
                    return;
                }

                await tableClient.CreateIfNotExistsAsync();

                // Validate the email address using Aspose.Email
                var emailValidator = new EmailValidator();
                emailValidator.Validate(email, out ValidationResult validationResult);

                bool isValid = validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess;
                string message = validationResult.Message ?? string.Empty;

                var entity = new EmailValidationEntity
                {
                    PartitionKey = "EmailValidation",
                    RowKey = Guid.NewGuid().ToString(),
                    Email = email,
                    IsValid = isValid,
                    Message = message
                };

                await tableClient.UpsertEntityAsync(entity);
                log.LogInformation($"Email '{email}' validation result stored. IsValid: {isValid}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Error processing email '{email}'.");
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Sample email list simulating a queue
            var emailQueue = new Queue<string>();
            emailQueue.Enqueue("valid.email@example.com");
            emailQueue.Enqueue("invalid-email");
            emailQueue.Enqueue(""); // empty entry

            var logger = new ConsoleLogger();
            var tableClient = new TableClient();

            while (emailQueue.Count > 0)
            {
                var email = emailQueue.Dequeue();
                await EmailValidationFunction.Run(email, logger, tableClient);
            }

            // Display stored results
            Console.WriteLine("\nStored validation results:");
            foreach (EmailValidationEntity entity in tableClient.GetAllEntities())
            {
                Console.WriteLine($"- Email: {entity.Email}, IsValid: {entity.IsValid}, Message: {entity.Message}");
            }
        }
    }
}
