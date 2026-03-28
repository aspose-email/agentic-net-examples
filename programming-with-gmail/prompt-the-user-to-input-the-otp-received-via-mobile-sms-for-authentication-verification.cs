using System;

namespace AsposeEmailOtpSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Prompt the user to enter the OTP received via mobile SMS
                Console.Write("Enter OTP: ");
                string otp = Console.ReadLine();

                // Validate input
                if (string.IsNullOrWhiteSpace(otp))
                {
                    Console.Error.WriteLine("Error: OTP not provided.");
                    return;
                }

                // Use the OTP as needed (placeholder for verification logic)
                Console.WriteLine($"OTP entered: {otp}");
            }
            catch (Exception ex)
            {
                // Global exception handling
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
