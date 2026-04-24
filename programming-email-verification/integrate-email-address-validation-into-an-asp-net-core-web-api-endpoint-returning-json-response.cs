using System;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Simple console simulation of a web API endpoint.
            // Expect the email address as the first command‑line argument.
            if (args == null || args.Length == 0)
            {
                Console.Error.WriteLine("Email address argument is missing.");
                return;
            }

            string emailAddress = args[0];

            // Perform validation using Aspose.Email's EmailValidator.
            EmailValidator validator = new EmailValidator();
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Build a response object.
            ValidationResponse response = new ValidationResponse
            {
                Email = emailAddress,
                IsValid = validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess,
                Message = validationResult.Message,
                ReturnCode = validationResult.ReturnCode
            };

            // Serialize response to JSON and write to console.
            string json = JsonSerializer.Serialize(response);
            Console.WriteLine(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper class representing the JSON response.
    private class ValidationResponse
    {
        public string Email { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public ValidationResponseCode ReturnCode { get; set; }
    }
}
