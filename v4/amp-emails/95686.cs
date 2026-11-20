using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        // AMP HTML content with CSRF hidden field, email input, and submit button
        string ampHtml = @"<!doctype html>
<html amp4email>
<head>
  <meta charset=""utf-8"">
  <script async src=""https://cdn.ampproject.org/v0.js""></script>
  <style amp4email-boilerplate>body{visibility:hidden}</style>
  <style amp-custom>
    .form-container {padding:10px;}
    input, button {margin:5px 0;}
  </style>
</head>
<body>
  <div class=""form-container"">
    <form method=""post"" action-xhr=""https://example.com/submit"">
      <input type=""hidden"" name=""csrf_token"" value=""{CSRF_TOKEN}"">
      <label>Email:</label><br>
      <input type=""email"" name=""email"" required><br>
      <button type=""submit"">Submit</button>
    </form>
  </div>
</body>
</html>";

        // Create the mail message
        using (AmpMessage message = new AmpMessage())
        {
            string csrfToken = Environment.GetEnvironmentVariable("AMP_CSRF_TOKEN");
            if (string.IsNullOrWhiteSpace(csrfToken))
            {
                Console.Error.WriteLine("Provide a server-issued AMP_CSRF_TOKEN before building the form.");
                return;
            }
            message.From = new MailAddress("sender@example.com");
            message.To.Add("recipient@example.com");
            message.Subject = "AMP Form with CSRF Token";
            message.HtmlBody = ampHtml.Replace("{CSRF_TOKEN}", csrfToken);
            AmpForm form = new AmpForm { Method = FormMethod.Post, Target = FormTarget.Top, ActionXhr = "https://example.com/submit" };
            form.Fieldset.Add(new FormField { InputType = "hidden", Name = "csrf_token", Value = csrfToken });
            form.Fieldset.Add(new FormField { InputType = "email", Name = "email", IsRequired = true });
            form.Fieldset.Add(new FormField { InputType = "submit", Value = "Submit" });
            message.AddAmpComponent(form);

            string outputPath = "amp_form_csrf.eml";

            try
            {
                // Ensure the output directory exists
                string dir = Path.GetDirectoryName(outputPath);
                if (string.IsNullOrEmpty(dir))
                {
                    dir = Directory.GetCurrentDirectory();
                }
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                // Save the message as an EML file
                message.Save(outputPath, SaveOptions.DefaultEml);
                Console.WriteLine($"Message saved to {outputPath}");
            }
            catch (IOException ioEx)
            {
                Console.Error.WriteLine($"IO error: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
