namespace Application.SalesFolder.Interfaces
{
    public interface IGetSalesByDateQuery
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string? CustomerName { get; set; }
        string? GoodsName { get; set; }
    }
}