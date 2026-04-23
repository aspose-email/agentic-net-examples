using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Guard input file existence
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the existing message into an AmpMessage
            using (FileStream inputStream = File.OpenRead(inputPath))
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.Import(inputStream);

                // Create a new AMP form component
                AmpForm ampForm = new AmpForm();
                ampForm.Action = "https://example.com/submit";
                ampForm.Method = FormMethod.Post; // Correct enum assignment

                // First input field: Name
                FormField nameField = new FormField();
                nameField.Name = "name";
                nameField.Label = "Name";
                nameField.InputType = "text";
                nameField.IsRequired = true;
                ampForm.Fieldset.Add(nameField);

                // Second input field: Email
                FormField emailField = new FormField();
                emailField.Name = "email";
                emailField.Label = "Email";
                emailField.InputType = "email";
                emailField.IsRequired = true;
                ampForm.Fieldset.Add(emailField);

                // Add the form component to the message
                ampMessage.AddAmpComponent(ampForm);

                // Guard output directory existence
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified message
                ampMessage.Save(outputPath);
                Console.WriteLine($"Message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
