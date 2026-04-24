using System;
using System.IO;
using System.Reflection;
using System.Collections;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the AMP message file
            string filePath = "ampMessage.eml";

            // Ensure the file exists; create a minimal placeholder if missing
            if (!File.Exists(filePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (AmpMessage placeholder = new AmpMessage())
                {
                    placeholder.Subject = "Sample AMP Message";
                    placeholder.Body = "This is a placeholder AMP message.";
                    // Save as EML (MailMessage format)
                    placeholder.Save(filePath);
                }
            }

            // Load the message from the file
            using (MailMessage loaded = MailMessage.Load(filePath))
            {
                // Cast to AmpMessage to access AMP‑specific members
                AmpMessage ampMessage = loaded as AmpMessage;
                if (ampMessage == null)
                {
                    Console.Error.WriteLine("The loaded message is not an AmpMessage.");
                    return;
                }

                // Attempt to retrieve the collection of AMP components via reflection
                PropertyInfo componentsProperty = typeof(AmpMessage).GetProperty(
                    "Components",
                    BindingFlags.Instance | BindingFlags.Public);

                if (componentsProperty != null)
                {
                    // The property is expected to be an IEnumerable of components
                    IEnumerable components = componentsProperty.GetValue(ampMessage) as IEnumerable;
                    if (components != null)
                    {
                        foreach (object component in components)
                        {
                            Console.WriteLine($"Component type: {component.GetType().FullName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No AMP components found in the message.");
                    }
                }
                else
                {
                    Console.WriteLine("AmpMessage does not expose a public Components property.");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            Console.Error.WriteLine(ex.Message);
        }
    }
}
