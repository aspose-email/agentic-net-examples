using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string pdfPath = "sample.pdf";
            string msgPath = "journal.msg";

            // Ensure PDF file exists; create a minimal placeholder if missing
            if (!File.Exists(pdfPath))
            {
                try
                {
                    // Very small PDF stub
                    string pdfStub = "%PDF-1.4\n%âãÏÓ\n1 0 obj\n<<>>\nendobj\ntrailer\n<<>>\n%%EOF";
                    File.WriteAllText(pdfPath, pdfStub);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PDF: {ex.Message}");
                    return;
                }
            }

            // Read PDF data
            byte[] pdfData;
            try
            {
                pdfData = File.ReadAllBytes(pdfPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read PDF file: {ex.Message}");
                return;
            }

            // Create a MAPI journal and set its description
            using (MapiJournal journal = new MapiJournal())
            {
                journal.Description = "Sample journal entry";

                // Attach the PDF document
                journal.Attachments.Add(Path.GetFileName(pdfPath), pdfData);

                // Save the journal to a MSG file
                try
                {
                    journal.Save(msgPath);
                    Console.WriteLine($"Journal saved to '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save journal: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
