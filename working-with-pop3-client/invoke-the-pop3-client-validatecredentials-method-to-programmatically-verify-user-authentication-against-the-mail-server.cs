using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with server details
            using (Pop3Client client = new Pop3Client("pop.example.com", "user@example.com", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Validate the supplied credentials against the POP3 server
                    bool isValid = client.ValidateCredentials();

                    Console.WriteLine(isValid
                        ? "Credentials are valid."
                        : "Credentials are invalid.");
                }
                catch (Exception ex)
                {
                    // Handle errors that occur during validation (e.g., network issues, authentication failures)
                    Console.Error.WriteLine($"Validation error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors in the overall execution
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
