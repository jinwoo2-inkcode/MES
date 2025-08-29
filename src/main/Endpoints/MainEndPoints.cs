namespace main.Endpoints;
using main.Data;

public static class MainEndPoints
{
     public static void AddCourseEndpoints(this WebApplication app)
    {
        app.MapGet("/load", LoadAllLoads);
        app.MapGet("/load/{Load_ID}", LoadCourseById);
    }

    private static IResult LoadAllLoads(
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

    private static IResult LoadCourseById(Load_Data data, string Load_ID)
    {
        var output = data.LoadData.SingleOrDefault(x => x.LOAD_ID == Load_ID);
        if (output is null)
            return Results.NotFound();

        return Results.Ok(output);
    }
}