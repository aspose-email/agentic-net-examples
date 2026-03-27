using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string configFilePath = "userconfig.txt";

            // Ensure the directory for the config file exists
            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(configFilePath));
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Load existing configuration into a dictionary
            Dictionary<string, string> settings = new Dictionary<string, string>();
            if (File.Exists(configFilePath))
            {
                try
                {
                    using (FileStream readStream = new FileStream(configFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (StreamReader reader = new StreamReader(readStream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            int separatorIndex = line.IndexOf('=');
                            if (separatorIndex > 0)
                            {
                                string key = line.Substring(0, separatorIndex);
                                string value = line.Substring(separatorIndex + 1);
                                settings[key] = value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error reading configuration: " + ex.Message);
                    return;
                }
            }

            // Prompt user for a configuration key and value
            Console.Write("Enter configuration key: ");
            string inputKey = Console.ReadLine();
            Console.Write("Enter configuration value: ");
            string inputValue = Console.ReadLine();

            // Update the settings dictionary
            settings[inputKey] = inputValue;

            // Save the updated settings back to the file
            try
            {
                using (FileStream writeStream = new FileStream(configFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(writeStream))
                {
                    foreach (KeyValuePair<string, string> kvp in settings)
                    {
                        writer.WriteLine(kvp.Key + "=" + kvp.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error writing configuration: " + ex.Message);
                return;
            }

            // Display all stored configuration values
            Console.WriteLine("Current configuration values:");
            foreach (KeyValuePair<string, string> kvp in settings)
            {
                Console.WriteLine(kvp.Key + " = " + kvp.Value);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}