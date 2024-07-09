namespace Application.Services.Sync
{
    public interface IInventoryServiceSync
    {
        bool UpdateInventory(int goodsId, int quantity);
    }
}