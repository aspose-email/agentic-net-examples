using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Specify the directory path to be created
            string directoryPath = @"C:\MyProject\Resources";

            // Check if the directory already exists
            if (!Directory.Exists(directoryPath))
            {
                // Create the directory and obtain a DirectoryInfo object
                DirectoryInfo createdDirectory = Directory.CreateDirectory(directoryPath);

                // Verify that the directory was created successfully
                if (!createdDirectory.Exists)
                {
                    Console.Error.WriteLine($"Error: Failed to create directory – {directoryPath}");
                    return;
                }
            }

            Console.WriteLine($"Directory is ready at: {directoryPath}");
        }
        catch (Exception exception)
        {
            // Output any unexpected errors
            Console.Error.WriteLine($"Error: {exception.Message}");
            return;
        }
    }
}
