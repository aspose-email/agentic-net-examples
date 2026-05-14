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
            // Define output HTML file path
            string outputPath = "task.html";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MAPI task
            using (MapiTask task = new MapiTask(
                subject: "Sample Task",
                body: "This is the body of the task.",
                startDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(5)))
            {
                // Set the history property (type of last change)
                task.History = MapiTaskHistory.Assigned;

                // Retrieve the underlying MAPI message to access HTML body
                using (MapiMessage underlyingMessage = task.GetUnderlyingMessage())
                {
                    // Build simple HTML content that includes task details and history
                    string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>{task.Subject}</title>
</head>
<body>
    <h1>{task.Subject}</h1>
    <p><strong>Start Date:</strong> {task.StartDate}</p>
    <p><strong>Due Date:</strong> {task.DueDate}</p>
    <p><strong>History:</strong> {task.History}</p>
    <h2>Body</h2>
    <div>{underlyingMessage.BodyHtml}</div>
</body>
</html>";

                    // Write the HTML to the file
                    try
                    {
                        File.WriteAllText(outputPath, htmlContent);
                        Console.WriteLine($"Task exported to HTML at: {Path.GetFullPath(outputPath)}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing HTML file: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
