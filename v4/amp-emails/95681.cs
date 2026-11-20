using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Amp;

namespace AmpFormExample
{
    class Program
    {
        static void Main()
        {
            string outputPath = "amp_form_subscribe.eml";

            try
            {
                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create an AMP message
                AmpMessage ampMessage = new AmpMessage();

                // Create a form with POST method
                AmpForm ampForm = new AmpForm();
                ampForm.Method = FormMethod.Post;
                ampForm.Action = "https://example.com/subscribe";

                // Create a submit button labeled "Subscribe"
                FormField submitButton = new FormField();
                submitButton.InputType = "submit";
                submitButton.Value = "Subscribe";

                // Add the button to the form
                ampForm.Fieldset.Add(submitButton);

                // Add the form to the AMP message
                ampMessage.AddAmpComponent(ampForm);

                // Save the AMP email to a file
                ampMessage.Save(outputPath);

                Console.WriteLine("AMP email saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
                return;
            }
        }
    }
}
