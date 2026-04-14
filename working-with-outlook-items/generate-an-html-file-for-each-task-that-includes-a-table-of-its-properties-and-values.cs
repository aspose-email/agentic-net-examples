using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace HtmlTaskGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define a list of tasks with sample data
                List<TaskInfo> tasks = new List<TaskInfo>();
                tasks.Add(new TaskInfo { Id = 1, Name = "TaskOne", Description = "First task description" });
                tasks.Add(new TaskInfo { Id = 2, Name = "TaskTwo", Description = "Second task description" });

                // Ensure the output directory exists
                string outputDirectory = "TaskHtml";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Generate an HTML file for each task
                foreach (TaskInfo task in tasks)
                {
                    string filePath = Path.Combine(outputDirectory, task.Name + ".html");
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.WriteLine("<!DOCTYPE html>");
                            writer.WriteLine("<html>");
                            writer.WriteLine("<head><meta charset=\"utf-8\"><title>" + task.Name + "</title></head>");
                            writer.WriteLine("<body>");
                            writer.WriteLine("<h1>" + task.Name + "</h1>");
                            writer.WriteLine("<table border=\"1\">");
                            writer.WriteLine("<tr><th>Property</th><th>Value</th></tr>");
                            writer.WriteLine("<tr><td>Id</td><td>" + task.Id + "</td></tr>");
                            writer.WriteLine("<tr><td>Name</td><td>" + task.Name + "</td></tr>");
                            writer.WriteLine("<tr><td>Description</td><td>" + task.Description + "</td></tr>");
                            writer.WriteLine("</table>");
                            writer.WriteLine("</body>");
                            writer.WriteLine("</html>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to write file '" + filePath + "': " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }

    public class TaskInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
