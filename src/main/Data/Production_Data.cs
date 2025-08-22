using System.Text.Json;
using main.Models;
//using main.Models;

namespace main.Data;

public class Production_Data
{
    public List<Production> ProductionData { get; private set; }

    public Production_Data()
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

        ProductionData = JsonSerializer.Deserialize<List<Production>>(json, options) ?? new();

        return;
    }

}