using System.Text.Json;
using main.Models;
//using main.Models;

namespace main.Data;

public class Load_Data
{
    public List<Load> LoadData { get; private set; }

    public Load_Data()
    {
        var options = new JsonSerializerOptions
        { PropertyNameCaseInsensitive = true };

        //Not decided how to work this, not correct code
        
        string filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "Data.json"
        );
        string json = File.ReadAllText(filePath);

        LoadData = JsonSerializer.Deserialize<List<Load>>(json, options) ?? new();

        return;
    }

    
}