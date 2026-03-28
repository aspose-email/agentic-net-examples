using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.html";

            if (!File.Exists(inputPath))
{
    try
    {
        string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML.";
        File.WriteAllText(inputPath, placeholder);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
        return;
    }
}


            using (MailMessage message = MailMessage.Load(inputPath))
            {
                MhtSaveOptions saveOptions = new MhtSaveOptions()
                {
                    SaveAttachments = true
                };

                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
