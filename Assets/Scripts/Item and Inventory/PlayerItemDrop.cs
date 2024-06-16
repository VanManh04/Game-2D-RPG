using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] protected float chanceToLooseItems;
    [SerializeField] protected float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        // list of equipment
        //foreach item we gonna check if shout losse item

        Inventory inventory = Inventory.instance;
        
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLosse = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);//tha item nhu luc giet quai

                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);

                materialsToLosse.Add(item);
            }
        }

        for (int i = 0; i < materialsToLosse.Count; i++)
        {
            inventory.RemoveItem(materialsToLosse[i].data);
        }
    }
}