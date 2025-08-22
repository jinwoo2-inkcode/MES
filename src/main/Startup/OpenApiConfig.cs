namespace main.Startup;

using Microsoft.VisualBasic;
using Scalar.AspNetCore;
using System.Data.SqlClient;
using System.Text;

public static class OpenAPIConfig
{

    //connection string variable - did not decide where to put this
    public static string ConnectionString = "Data Source=FREEPDB1;Initial Catalog=MES;User id=dbo;Password=Duksu123!;";

    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
    }


    public static void UseOpenApi(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "The sample api"; //title of the screen
                options.Theme = ScalarTheme.Moon; //this changes the color theme of the screen
                options.Layout = ScalarLayout.Modern; //layout of screen
                options.HideClientButton = true; //hides client

            }
            ); //this is the scalar nuget add-on
        }
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
    Name: StartOrder()
    Summary: 
    param:
    returns: 
    **/
    public static string[] Get()
    {
        string[] arr = ["", ""];
        return arr;
    }

    /**
    Name: StartOrder()
    Summary: Start a new order; Initial order is made with only mfg_order_nbr, Product_Id, Product_Desc, Make_Qty
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Make_Qty, Made_Qty, Due_Date,Start_Date, End_Date, Shipping_Loc, Mfg_Order_Nbr
    returns: null
    **/
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
        sql.Append($" VALUES ({PRODUCT_ID}, {PRODUCT_DESC}, {DEMAND_ORD_NBR}, {MAKE_QTY}, {MADE_QTY}, {DUE_DATE}, {START_DATE}, {END_DATE}, {SHIPPING_LOC}, {MFG_ORDER_NBR})");

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
                //MessageBox.Show("ERROR:" + ex.Message);
            }
        }
    }

    /**
    Name: UpdateOrder()
    Summary: Update existing order; Identified based on Mfg_Order_Nbr (non-changable); can be used to update a completed order or move loads etc;
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Make_Qty, Made_Qty, Due_Date,Start_Date, End_Date, Shipping_Loc, Mfg_Order_Nbr
    returns: null
    **/
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
        sql.Append($" SET PRODUCT_ID={PRODUCT_ID}, PRODUCT_DESC={PRODUCT_DESC}, DEMAND_ORD_NBR={DEMAND_ORD_NBR}, MAKE_QTY={MAKE_QTY}, MADE_QTY={MADE_QTY}, DUE_DATE={DUE_DATE}, START_DATE={START_DATE}, END_DATE={END_DATE}, SHIPPING_LOC={SHIPPING_LOC}");
        sql.Append($" WHERE MFG_ORDER_NBR = {MFG_ORDER_NBR}");


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
                //MessageBox.Show("ERROR:" + ex.Message);
            }
        }
    }

    /**
    Name: StartLoad()
    Summary: Create new load; 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
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
        sql.Append($" VALUES ({PRODUCT_ID}, {PRODUCT_DESC}, {DEMAND_ORD_NBR}, {LOAD_QTY}, {CREATE_DATETIME}, {LOAD_ID}, {ACTIVE})");

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
                //MessageBox.Show("ERROR:" + ex.Message);
            }
        }
    }

    /**
    Name: UpdateLoad()
    Summary: Update existing load; Identified based on Load_Id (non-changable); 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
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
        sql.Append($" SET PRODUCT_ID={PRODUCT_ID}, PRODUCT_DESC={PRODUCT_DESC}, DEMAND_ORD_NBR={DEMAND_ORD_NBR}, CREATE_DATETIME={CREATE_DATETIME}, LOAD_QTY={LOAD_QTY}, ACTIVE={ACTIVE}");
        sql.Append($" WHERE LOAD_ID = {LOAD_ID}");

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
                //MessageBox.Show("ERROR:" + ex.Message);
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