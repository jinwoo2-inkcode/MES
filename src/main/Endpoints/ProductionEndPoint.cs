namespace main.Endpoints;

using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

//Will not be worked on until format is finalized, need to decide on labor time and machine runtime

public static class ProductionEndPoints
{
    //connection string variable - did not decide where to put this
    [Obsolete]
    public static void AddProductionEndpoints(this WebApplication app)
    {

        app.MapGet("/production/StartProduction", StartProduction);
        app.MapGet("/production/UpdateProduction", UpdateProduction);
    }

    /**
    Name: StartProduction()
    Summary: Start a production; 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, workcenter, Due_Date
    returns: null
    **/
    [Obsolete]
    public static void StartProduction(string PRODUCT_ID,
                                string PRODUCT_DESC,
                                string DEMAND_ORD_NBR,
                                string WORKCENTER,
                                DateTime DUE_DATE)
    {
        const string sql = @"
            INSERT INTO dbo.MES_PRODUCTION_PLAN (PRODUCT_ID, PRODUCT_DESC, DEMAND_ORD_NBR, WORKCENTER, DUE_DATE)
            VALUES (@PRODUCT_ID, @PRODUCT_DESC, @DEMAND_ORD_NBR, @WORKCENTER, @DUE_DATE)";

        using var cnn = new SqlConnection(Functional.DatabaseConnectionString());
        using var cmd = new SqlCommand(sql, cnn);
        cmd.Parameters.Add("@PRODUCT_ID", SqlDbType.VarChar).Value = PRODUCT_ID;
        cmd.Parameters.Add("@PRODUCT_DESC", SqlDbType.VarChar).Value = PRODUCT_DESC;
        cmd.Parameters.Add("@DEMAND_ORD_NBR", SqlDbType.VarChar).Value = DEMAND_ORD_NBR;
        cmd.Parameters.Add("@WORKCENTER", SqlDbType.Int).Value = WORKCENTER;
        cmd.Parameters.Add("@DUE_DATE", SqlDbType.DateTime2).Value = DUE_DATE;

        cnn.Open();
        using var reader = cmd.ExecuteReader();

    }

    /**
    Name: UpdateProduction()
    Summary: Update existing load; Identified based on Load_Id (non-changable); 
    param: Product_Id,Product_Desc, Demand_Ord_Nbr, Load_Qty, Create_Date, Load_Id, active
    returns: null
    **/
    [Obsolete]
    public static void UpdateProduction(string? PRODUCT_ID,
                                string? PRODUCT_DESC,
                                string DEMAND_ORD_NBR,
                                string? WORKCENTER,
                                DateTime? DUE_DATE)
    {
        string sql = @"
            UPDATE dbo.MES_PRODUCTION_PLAN
             SET ";
        if (PRODUCT_ID != null)
            sql += "PRODUCT_ID ='" + PRODUCT_ID + "',";
        if (PRODUCT_DESC != null)
            sql += "PRODUCT_DESC ='" + PRODUCT_DESC + "',";
        if (WORKCENTER != null)
            sql += "WORKCENTER = " + WORKCENTER + ",";
        if (DUE_DATE != null)
            sql += "DUE_DATE = " + DUE_DATE + ",";

        sql = sql.Remove(sql.Length - 1) + " WHERE DEMAND_ORD_NBR = '" + DEMAND_ORD_NBR + "'";

        using var cnn = new SqlConnection(Functional.DatabaseConnectionString());
        using var cmd = new SqlCommand(sql, cnn);

        cnn.Open();
        using var reader = cmd.ExecuteReader();

    }
}
