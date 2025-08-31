namespace main.Endpoints;

using main.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;
/*
//AWS
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
//IAmazonS3 s3Client = new AmazonS3Client(); // Uses default credential chain
// Or with specific credentials:
IAmazonS3 s3Client = new AmazonS3Client(credentials);
public class S3Example
{
    public async Task ListS3Buckets()
    {
        using (var client = new AmazonS3Client())
        {
            ListBucketsResponse response = await client.ListBucketsAsync();
            foreach (var bucket in response.Buckets)
            {
                Console.WriteLine(bucket.BucketName);
            }
        }
    }
}
*/
public static class MainEndPoints
{
    //connection string variable - did not decide where to put this
    public static string ConnectionString = "Server=mes.cgv4qeum0qbf.us-east-1.rds.amazonaws.com,1433;Database=MES;Integrated Security = false;User ID=admin;Password=Duksu123!;";
    public static void AddLoadEndpoints(this WebApplication app)
    {
        app.MapGet("/load", GetAllLoads);
        app.MapGet("/load/{L_ID}", GetLoad);
    }
    public static void AddOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/order", GetAllOrders);
        app.MapGet("/order/{MO_Order}", GetOrder);
    }
    public static void CheckConnection(this WebApplication app)
    {
        app.MapGet("/connection", IsConnectionStringSyntacticallyValid);
    }

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

    /*
    private static IResult GetAllLoads(
        Load_Data data,
        string? PRODUCT_ID,
        string? DEMAND_ORD_NBR,
        string? Load_ID,
        double? Load_Qty,
        DateTime? Create_Datetime,
        int? active
        )
    {
        var output = data.LoadData;
        if (!string.IsNullOrWhiteSpace(PRODUCT_ID))
        {
            output.RemoveAll(x => string.Compare(
                x.PRODUCT_ID,
                PRODUCT_ID,
                StringComparison.OrdinalIgnoreCase) != 0);
        }
        if (!string.IsNullOrWhiteSpace(DEMAND_ORD_NBR))
        {
            output.RemoveAll(x => string.Compare(
                x.DEMAND_ORD_NBR,
                DEMAND_ORD_NBR,
                StringComparison.OrdinalIgnoreCase) != 0);
        }


        return Results.Ok(output);
    }
    */

    private static IResult LoadByLoadId(Load_Data data, string Load_ID)
    {
        var output = data.LoadData.SingleOrDefault(x => x.LOAD_ID == Load_ID);
        if (output is null)
            return Results.NotFound();

        return Results.Ok(output);
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

    /**
    Name: GetOrder()
    Summary: Get the mfg order information associated to a given mfg_order_nbr
    param: MO_Order
    returns: JsonResults
    **/
    [Obsolete]
    public static string GetAllOrders(DateTime? start_datetime, DateTime? end_datetime)
    {
        DateTime e_d = end_datetime ?? DateTime.Now;
        DateTime s_d = start_datetime ?? e_d.AddDays(-1);

        StringBuilder sql = new StringBuilder();

        string JsonResults = "";

        sql.Append("SELECT * FROM MES_ORDER");
        sql.Append($" WHERE START_DATE BETWEEN '{s_d}' AND '{e_d}'");

        SqlConnection cnn = new SqlConnection(DatabaseConnectionString());
        cnn.Open();
        SqlCommand cmd = new SqlCommand(sql.ToString(), cnn);

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                JsonResults = SqlDataToJson(reader);
            }

        }
        cnn.Close();

        return JsonResults;
    }
    /**
    Name: GetOrder()
    Summary: Get the mfg order information associated to a given mfg_order_nbr
    param: MO_Order
    returns: JsonResults
    **/
    [Obsolete]
    public static string GetOrder(string MO_Order)
    {
        StringBuilder sql = new StringBuilder();

        string JsonResults = "";

        sql.Append("SELECT * FROM MES_ORDER");
        sql.Append($" WHERE MFG_ORDER_NBR = '{MO_Order}'");

        SqlConnection cnn = new SqlConnection(DatabaseConnectionString());
        cnn.Open();
        SqlCommand cmd = new SqlCommand(sql.ToString(), cnn);

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {

                JsonResults = SqlDataToJson(reader);
                /*
                // Access data by column name or index
                string P_ID = reader["PRODUCT_ID"].ToString();
                string P_Desc = reader["PRODUCT_DESC"].ToString();
                string DO_Nbr = reader["DEMAND_ORD_NBR"].ToString() ?? "";
                string ShipLoc = reader["SHIPPING_LOC"].ToString();
                string MO_NBR = reader["MFG_ORDER_NBR"].ToString();
                int make = Convert.ToInt32(reader["MAKE_QTY"]);
                int made = Convert.ToInt32(reader["MADE_QTY"]);
                DateTime due = Convert.ToDateTime(reader["DUE_DATE"]);
                DateTime start_d = Convert.ToDateTime(reader["START_DATE"]);
                DateTime end_d = Convert.ToDateTime(reader["END_DATE"]);
                // Process the retrieved data
                */
            }

        }
        cnn.Close();

        return JsonResults;
    }
    /**
    Name: StartOrder()
    Summary: Start a new order; Initial order is made with only mfg_order_nbr, Product_Id, Product_Desc, Make_Qty
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Make_Qty, Made_Qty, Due_Date,Start_Date, End_Date, Shipping_Loc, Mfg_Order_Nbr
    returns: null
    **/
    [Obsolete]
    public static void StartOrder(string P_Id,
                                    string P_Desc,
                                    string DO_Nbr,
                                    double make,
                                    double made,
                                    DateTime DueDate,
                                    DateTime StartDate,
                                    DateTime EndDate,
                                    string ShipLoc,
                                    string MO_Nbr)
    {

        StringBuilder sql = new StringBuilder();

        string PRODUCT_ID = P_Id;
        string PRODUCT_DESC = P_Desc;
        string DEMAND_ORD_NBR = DO_Nbr;
        double MAKE_QTY = make;
        double MADE_QTY = made; //usually 0
        DateTime DUE_DATE = DueDate;
        DateTime START_DATE = StartDate; //usually null
        DateTime END_DATE = EndDate; //usually null
        string SHIPPING_LOC = ShipLoc;
        string MFG_ORDER_NBR = MO_Nbr;

        sql.Append("INSERT INTO dbo.MES_ORDER (PRODUCT_ID, PRODUCT_DESC, DEMAND_ORD_NBR, MAKE_QTY, MADE_QTY, DUE_DATE, START_DATE, END_DATE, SHIPPING_LOC, MFG_ORDER_NBR)");
        sql.Append($" VALUES ('{PRODUCT_ID}', '{PRODUCT_DESC}', '{DEMAND_ORD_NBR}', {MAKE_QTY}, {MADE_QTY}, '{DUE_DATE}', '{START_DATE}', '{END_DATE}', '{SHIPPING_LOC}', '{MFG_ORDER_NBR}')");

        using (SqlConnection cnn = new SqlConnection(DatabaseConnectionString()))
        {
            try
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cnn))
                {
                    cmd.ExecuteNonQuery();

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }
    }
    /**
    Name: UpdateOrder()
    Summary: Update existing order; Identified based on Mfg_Order_Nbr (non-changable); can be used to update a completed order or move loads etc;
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Make_Qty, Made_Qty, Due_Date,Start_Date, End_Date, Shipping_Loc, Mfg_Order_Nbr
    returns: null
    **/
    [Obsolete]
    public static void UpdateOrder(string P_Id,
                                    string P_Desc,
                                    string DO_Nbr,
                                    double make,
                                    double made,
                                    DateTime DueDate,
                                    DateTime StartDate,
                                    DateTime EndDate,
                                    string ShipLoc,
                                    string MO_Nbr)
    {

        StringBuilder sql = new StringBuilder();

        string PRODUCT_ID = P_Id;
        string PRODUCT_DESC = P_Desc;
        string DEMAND_ORD_NBR = DO_Nbr;
        double MAKE_QTY = make;
        double MADE_QTY = made;
        DateTime DUE_DATE = DueDate;
        DateTime START_DATE = StartDate;
        DateTime END_DATE = EndDate;
        string SHIPPING_LOC = ShipLoc;
        string MFG_ORDER_NBR = MO_Nbr;

        sql.Append("UPDATE dbo.MES_ORDER");
        sql.Append($" SET PRODUCT_ID='{PRODUCT_ID}', PRODUCT_DESC='{PRODUCT_DESC}', DEMAND_ORD_NBR='{DEMAND_ORD_NBR}', MAKE_QTY={MAKE_QTY}, MADE_QTY={MADE_QTY}, DUE_DATE='{DUE_DATE}', START_DATE='{START_DATE}', END_DATE='{END_DATE}', SHIPPING_LOC='{SHIPPING_LOC}'");
        sql.Append($" WHERE MFG_ORDER_NBR = '{MFG_ORDER_NBR}'");


        using (SqlConnection cnn = new SqlConnection(DatabaseConnectionString()))
        {
            try
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cnn))
                {
                    cmd.ExecuteNonQuery();

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                // We should log the error somewhere, 
                // for this example let's just show a message
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }
    }

    // [Obsolete]
    // public static string GetAllLoads(DateTime? start_datetime, DateTime? end_datetime)
    // {
    //     DateTime e_d = end_datetime ?? DateTime.Now;
    //     DateTime s_d = start_datetime ?? e_d.AddDays(-1);

    //     StringBuilder sql = new StringBuilder();


    //     string JsonResults = "";

    //     sql.Append("SELECT * FROM MES_LOAD");
    //     sql.Append($" WHERE CREATE_DATETIME BETWEEN '{s_d}' AND '{e_d}'");

    //     SqlConnection cnn = new SqlConnection(DatabaseConnectionString());
    //     cnn.Open();
    //     SqlCommand cmd = new SqlCommand(sql.ToString(), cnn);

    //     using (SqlDataReader reader = cmd.ExecuteReader())
    //     {
    //         while (reader.Read())
    //         {
    //             JsonResults = SqlDataToJson(reader);
    //         }

    //     }
    //     cnn.Close();

    //     return JsonResults;
    // }

    /**
    Name: GetAllLoad()
    Summary: Get all load information associated to a given time frame
    param: start_datetime, end_datetime
    returns: JsonResults
    **/

    // Fix : replaced string-concatenated DateTime literals with typed SQL parameters
    [Obsolete]
    public static string GetAllLoads(DateTime? start_datetime, DateTime? end_datetime)
    {
        DateTime e_d = end_datetime ?? DateTime.Now;
        DateTime s_d = start_datetime ?? e_d.AddDays(-1);

        const string sql = @"
            SELECT * 
            FROM MES_LOAD
            WHERE CREATE_DATETIME BETWEEN @s AND @e";

        using var cnn = new SqlConnection(DatabaseConnectionString());
        using var cmd = new SqlCommand(sql, cnn);
        cmd.Parameters.Add("@s", SqlDbType.DateTime2).Value = s_d;
        cmd.Parameters.Add("@e", SqlDbType.DateTime2).Value = e_d;

        cnn.Open();
        using var reader = cmd.ExecuteReader();

        // load all rows at once, no while(reader.Read()) loop
        var dt = new DataTable();
        dt.Load(reader);
        return JsonConvert.SerializeObject(dt);
    }
    /**
    Name: GetLoad()
    Summary: Get the load information associated to a given load ID
    param: L_ID
    returns: JsonResults
    **/
    [Obsolete]
    public static string GetLoad(string L_ID)
    {
        StringBuilder sql = new StringBuilder();


        string JsonResults = "";

        sql.Append("SELECT * FROM MES_LOAD");
        sql.Append($" WHERE LOAD_ID = '{L_ID}'");
        using (SqlConnection cnn = new SqlConnection(DatabaseConnectionString()))
        {
            cnn.Open();
            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cnn))
            { 
                
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    JsonResults = SqlDataToJson(reader);
                }
            }
            cnn.Close();
        }
        return JsonResults;
    }
    /**
    Name: StartLoad()
    Summary: Create new load; 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
    [Obsolete]
    public static void StartLoad(string P_Id,
                                string P_Desc,
                                string DO_Nbr,
                                double L_Qty,
                                DateTime C_Date,
                                string L_Id,
                                int active)
    {
        StringBuilder sql = new StringBuilder();

        string PRODUCT_ID = P_Id;
        string PRODUCT_DESC = P_Desc;
        string DEMAND_ORD_NBR = DO_Nbr;
        double LOAD_QTY = L_Qty;
        DateTime CREATE_DATETIME = C_Date;
        string LOAD_ID = L_Id;
        int ACTIVE = active;

        sql.Append("INSERT INTO dbo.MES_ORDER (PRODUCT_ID, PRODUCT_DESC, DEMAND_ORD_NBR, LOAD_QTY, CREATE_DATETIME, LOAD_ID, ACTIVE)");
        sql.Append($" VALUES ('{PRODUCT_ID}', '{PRODUCT_DESC}', '{DEMAND_ORD_NBR}', '{LOAD_QTY}', '{CREATE_DATETIME}', '{LOAD_ID}', '{ACTIVE}')");

        using (SqlConnection cnn = new SqlConnection(DatabaseConnectionString()))
        {
            try
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cnn))
                {
                    cmd.ExecuteNonQuery();

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                // We should log the error somewhere, 
                // for this example let's just show a message
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }
    }

    /**
    Name: UpdateLoad()
    Summary: Update existing load; Identified based on Load_Id (non-changable); 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
    [Obsolete]
    public static void UpdateLoad(string P_Id,
                                string P_Desc,
                                string DO_Nbr,
                                double L_Qty,
                                DateTime C_Date,
                                string L_Id,
                                int active)
    {
        StringBuilder sql = new StringBuilder();

        string PRODUCT_ID = P_Id;
        string PRODUCT_DESC = P_Desc;
        string DEMAND_ORD_NBR = DO_Nbr;
        double LOAD_QTY = L_Qty;
        DateTime CREATE_DATETIME = C_Date;
        string LOAD_ID = L_Id;
        int ACTIVE = active;

        sql.Append("UPDATE dbo.MES_LOAD");
        sql.Append($" SET PRODUCT_ID='{PRODUCT_ID}', PRODUCT_DESC='{PRODUCT_DESC}', DEMAND_ORD_NBR='{DEMAND_ORD_NBR}', CREATE_DATETIME='{CREATE_DATETIME}', LOAD_QTY={LOAD_QTY}, ACTIVE={ACTIVE}");
        sql.Append($" WHERE LOAD_ID = '{LOAD_ID}'");

        using (SqlConnection cnn = new SqlConnection(DatabaseConnectionString()))
        {
            try
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cnn))
                {
                    cmd.ExecuteNonQuery();

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                // We should log the error somewhere, 
                // for this example let's just show a message
                Console.WriteLine("ERROR:" + ex.Message);
            }
        }
    }
}



/**
    Summary: 
    param:
    returns: 
**/

//reference from microsoft fro api formats
/*
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.Name}\tPrice: " +
                $"{product.Price}\tCategory: {product.Category}");
        }

        static async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }

        static async Task<Product> UpdateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Product>();
            return product;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Product product = new Product
                {
                    Name = "Gizmo",
                    Price = 100,
                    Category = "Widgets"
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine($"Created at {url}");

                // Get the product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Update the product
                Console.WriteLine("Updating price...");
                product.Price = 80;
                await UpdateProductAsync(product);

                // Get the updated product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Delete the product
                var statusCode = await DeleteProductAsync(product.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
*/

//This is an example of writing a query
/*
string connetionString = null;
string sql = null;

// All the info required to reach your db. See connectionstrings.com
connetionString = "Data Source=UMAIR;Initial Catalog=Air; Trusted_Connection=True;" ;

// Prepare a proper parameterized query 
sql = "insert into Main ([Firt Name], [Last Name]) values(@first,@last)";

// Create the connection (and be sure to dispose it at the end)
using(SqlConnection cnn = new SqlConnection(connetionString))
{
    try
    {
       // Open the connection to the database. 
       // This is the first critical step in the process.
       // If we cannot reach the db then we have connectivity problems
       cnn.Open();

       // Prepare the command to be executed on the db
       using(SqlCommand cmd = new SqlCommand(sql, cnn))
       {
           // Create and set the parameters values 
           cmd.Parameters.Add("@first", SqlDbType.NVarChar).Value = textbox2.text;
           cmd.Parameters.Add("@last", SqlDbType.NVarChar).Value = textbox3.text;

           // Let's ask the db to execute the query
           int rowsAdded = cmd.ExecuteNonQuery();
           if(rowsAdded > 0) 
              MessageBox.Show ("Row inserted!!" + );
           else
              // Well this should never really happen
              MessageBox.Show ("No row inserted");

       }
    }
    catch(Exception ex)
    {
        // We should log the error somewhere, 
        // for this example let's just show a message
        MessageBox.Show("ERROR:" + ex.Message);
    }
}
*/