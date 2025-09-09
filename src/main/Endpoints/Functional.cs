namespace main.Endpoints;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

public static class Functional
{
    //connection string variable - did not decide where to put this
    public static string ConnectionString = "";

    [Obsolete]
    public static void CheckConnection(this WebApplication app)
    {
        app.MapGet("/connection", IsConnectionStringSyntacticallyValid);
    }
    [Obsolete]
    public static bool IsConnectionStringSyntacticallyValid()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            return false; // Or throw ArgumentException
        }

        try
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);


            return true;
        }
        catch (Exception ex)
        {
            // Log or handle the exception (e.g., KeyNotFoundException, FormatException, ArgumentException)
            Console.WriteLine($"Connection string format error: {ex.Message}");
            return false;
        }
    }

    /**
    Name: SqlDataToJson()
    Summary: Function to convert an SqlDataReader datatype to a json string
    param: dataReader
    returns: JSONString
    **/
    [Obsolete]
    public static String SqlDataToJson(SqlDataReader dataReader)
    {
        var dataTable = new DataTable();
        dataTable.Load(dataReader);
        string JSONString = string.Empty;
        JSONString = JsonConvert.SerializeObject(dataTable);
        return JSONString;
    }

    /**
    Name: DatabaseConnectionString()
    Summary: Database string to be used for sql
    param: null
    returns: database string
    **/
    public static string DatabaseConnectionString()
    {
        return ConnectionString;
    }
    
}
