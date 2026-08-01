using System;

namespace ProgrammingWithGmail
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter the OTP received via SMS: ");
            string otp = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(otp))
            {
                Console.WriteLine("No OTP entered. Exiting.");
                return;
            }

            // Here you would normally verify the OTP with your authentication service.
            // For demonstration, we just acknowledge the input.
            Console.WriteLine($"OTP entered: {otp}");
        }
    }
}
