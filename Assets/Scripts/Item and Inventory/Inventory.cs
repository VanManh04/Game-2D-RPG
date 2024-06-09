using System.Collections.Generic;
using UnityEngine;
//Inventory: Day la lop quan ly kho do nguoi choi
public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventory;  //Danh sach cac vat pham trong kho
    public Dictionary<ItemData, InventoryItem> inventoryDictianory; //Tu dien de nhanh chong tra cuu cac vat pham theo ItemData

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictianory;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictianory = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictianory = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)//add type Equipment
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictianory.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))//Tim item trong InventoryItem neu co thi true
        {
            value.AddStack();
        }
        else//Khong ton tai trong kho thi tao 1 muc moi va luu vao
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictianory.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))//Tim item trong InventoryItem neu co thi true
        {
            if (value.stackSize <= 1)//Kiem tra so luong item <= 1 thi xoa khoi danh sach inventoryItems va loai bo khoi tu dien inventoryDictionary
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else//Khong thi tru so luong
                value.RemoveStack();
        }

        if (stashDictianory.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (value.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictianory.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }
}
