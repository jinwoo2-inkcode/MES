namespace main.Data;

using System.Text.Json;
using main.Models;

//this file needs working

public class CourseData
{
    public List<Load> load { get; set; }

    public CourseData()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
        "Data",
        "coursedata.json");

        string json = File.ReadAllText(filePath);

        load = JsonSerializer.Deserialize<List<Load>>(json, options) ?? new();
    }
}