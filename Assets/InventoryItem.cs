using System;
//InventoryItem: Lop nay dai dien cho 1 vat pham trong kho, bao bao gom du lieu cua vat pham (ItemData) va so luong ngan sep (stackSize).
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        //TODO: add to stack
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}