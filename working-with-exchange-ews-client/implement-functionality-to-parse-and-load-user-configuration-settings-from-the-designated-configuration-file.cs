using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            string configFilePath = "userconfig.txt";

            if (!File.Exists(configFilePath))
            {
                Console.Error.WriteLine("Configuration file not found: " + configFilePath);
                return;
            }

            Dictionary<string, string> settings = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(configFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    // Skip empty lines and comments
                    if (line.Length == 0 || line.StartsWith("#"))
                    {
                        continue;
                    }

                    int separatorIndex = line.IndexOf('=');
                    if (separatorIndex <= 0 || separatorIndex == line.Length - 1)
                    {
                        Console.Error.WriteLine("Invalid configuration line: " + line);
                        continue;
                    }

                    string key = line.Substring(0, separatorIndex).Trim();
                    string value = line.Substring(separatorIndex + 1).Trim();

                    if (!settings.ContainsKey(key))
                    {
                        settings.Add(key, value);
                    }
                    else
                    {
                        settings[key] = value;
                    }
                }
            }

            Console.WriteLine("Loaded configuration settings:");
            foreach (KeyValuePair<string, string> entry in settings)
            {
                Console.WriteLine(entry.Key + " = " + entry.Value);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}