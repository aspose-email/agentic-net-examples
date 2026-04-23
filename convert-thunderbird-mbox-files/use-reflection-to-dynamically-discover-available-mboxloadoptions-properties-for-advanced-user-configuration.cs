using System;
using System.IO;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create a minimal placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                File.WriteAllText(mboxPath, string.Empty);
                Console.WriteLine($"Created placeholder MBOX file at {mboxPath}");
            }

            // Discover available MboxLoadOptions properties via reflection.
            MboxLoadOptions loadOptions = new MboxLoadOptions();
            PropertyInfo[] properties = typeof(MboxLoadOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine("MboxLoadOptions properties and their default values:");
            foreach (PropertyInfo prop in properties)
            {
                object value = prop.GetValue(loadOptions);
                Console.WriteLine($"- {prop.Name}: {(value ?? "null")}");
            }

            // Create the MBOX reader with the discovered options.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
