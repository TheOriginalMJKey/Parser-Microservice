namespace Application.Services.Async
{
    public interface IInventoryServiceAsync
    {
        Task<bool> UpdateInventoryAsync(int goodsId, int quantity);
    }
}