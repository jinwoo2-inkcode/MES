namespace main.Endpoints;

using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

public static class LoadEndPoints
{
    //connection string variable - did not decide where to put this
    [Obsolete]
    public static void AddLoadEndpoints(this WebApplication app)
    {
        app.MapGet("/load", GetAllLoads);
        app.MapGet("/load/{LOAD_ID}", GetLoad);
        app.MapGet("/load/StartLoad", StartLoad);
        app.MapGet("/load/UpdateLoad", UpdateLoad);
    }
    
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
    }

    private static string DatabaseConnectionString()
    {
        throw new NotImplementedException();
    }

    /**
    Name: GetLoad()
    Summary: Get the load information associated to a given load ID
    param: L_ID
    returns: JsonResults
    **/
    [Obsolete]
    public static string GetLoad(string Load_ID)
    {
        const string sql = @"
            SELECT * 
            FROM MES_LOAD
             WHERE LOAD_ID = @l";

        using var cnn = new SqlConnection(DatabaseConnectionString());
        using var cmd = new SqlCommand(sql, cnn);
        cmd.Parameters.Add("@l", SqlDbType.VarChar).Value = Load_ID;

        cnn.Open();
        using var reader = cmd.ExecuteReader();

        // load all rows at once, no while(reader.Read()) loop
        var dt = new DataTable();
        dt.Load(reader);
        return JsonConvert.SerializeObject(dt);
        /*
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
        */

    }
    /**
    Name: StartLoad()
    Summary: Create new load; 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
    [Obsolete]
    public static void StartLoad(string PRODUCT_ID,
                                string PRODUCT_DESC,
                                string DEMAND_ORD_NBR,
                                double LOAD_QTY,
                                //DateTime CREATE_DATETIME,
                                string LOAD_ID,
                                int ACTIVE)
    {
        const string sql = @"
            INSERT INTO dbo.MES_LOAD (PRODUCT_ID, PRODUCT_DESC, DEMAND_ORD_NBR, LOAD_QTY, CREATE_DATETIME, LOAD_ID, ACTIVE) 
            VALUES (@PRODUCT_ID, @PRODUCT_DESC, @DEMAND_ORD_NBR, @LOAD_QTY, GETDATE(), @LOAD_ID, @ACTIVE)";

        using var cnn = new SqlConnection(DatabaseConnectionString());
        using var cmd = new SqlCommand(sql, cnn);
        cmd.Parameters.Add("@PRODUCT_ID", SqlDbType.VarChar).Value = PRODUCT_ID;
        cmd.Parameters.Add("@PRODUCT_DESC", SqlDbType.VarChar).Value = PRODUCT_DESC;
        cmd.Parameters.Add("@DEMAND_ORD_NBR", SqlDbType.VarChar).Value = DEMAND_ORD_NBR;
        cmd.Parameters.Add("@LOAD_QTY", SqlDbType.Int).Value = LOAD_QTY;
        //cmd.Parameters.Add("@CREATE_DATETIME", SqlDbType.DateTime2).Value = CREATE_DATETIME;
        cmd.Parameters.Add("@LOAD_ID", SqlDbType.VarChar).Value = LOAD_ID;
        cmd.Parameters.Add("@ACTIVE", SqlDbType.SmallInt).Value = ACTIVE;

        cnn.Open();
        using var reader = cmd.ExecuteReader();
        /*
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
        */


    }

    /**
    Name: UpdateLoad()
    Summary: Update existing load; Identified based on Load_Id (non-changable); 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
    [Obsolete]
    public static void UpdateLoad(string? PRODUCT_ID,
                                string? PRODUCT_DESC,
                                string? DEMAND_ORD_NBR,
                                double? LOAD_QTY,
                                DateTime? CREATE_DATETIME,
                                string LOAD_ID,
                                int? ACTIVE)
    {
        string sql = @"
            UPDATE dbo.MES_LOAD
             SET ";
        if (PRODUCT_ID != null)
            sql += "PRODUCT_ID ='" + PRODUCT_ID + "',";
        if (PRODUCT_DESC != null)
            sql += "PRODUCT_DESC ='" + PRODUCT_DESC + "',";
        if (DEMAND_ORD_NBR != null)
            sql += "DEMAND_ORD_NBR ='" + DEMAND_ORD_NBR + "',";
        if (LOAD_QTY != null)
            sql += "LOAD_QTY = " + LOAD_QTY + ",";
        if (ACTIVE != null)
            sql += "ACTIVE = " + ACTIVE + ",";

        sql = sql.Remove(sql.Length - 1) + " WHERE LOAD_ID = '" + LOAD_ID + "'";
        
        using var cnn = new SqlConnection(DatabaseConnectionString());
        using var cmd = new SqlCommand(sql, cnn);
        
        cnn.Open();
        using var reader = cmd.ExecuteReader();
        
        /*
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
        */
    }
}
