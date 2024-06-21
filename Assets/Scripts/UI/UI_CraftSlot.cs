using System.Diagnostics;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null)
            return;

        item.data = _data;

        itemImage.sprite = _data.icon;
        itemText.text = _data.name;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        //inventory craft item data
        //ItemData_Equipment craftData = item.data as ItemData_Equipment;

        //Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);

        ui.craftWindow.SetupCraftWindown(item.data as ItemData_Equipment);
    }
}
