using System.Collections.Generic;
using UnityEngine;
//Inventory: Day la lop quan ly kho do nguoi choi
public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;  //Danh sach cac vat pham trong kho
    public Dictionary<ItemData, InventoryItem> inventoryDictianory; //Tu dien de nhanh chong tra cuu cac vat pham theo ItemData

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    private UI_ItemSlot[] itemSlot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictianory = new Dictionary<ItemData, InventoryItem>();

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlot[i].UpdateSlot(inventoryItems[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))//Tim item trong InventoryItem neu co thi true
        {
            value.AddStack();
        }
        else//Khong ton tai trong kho thi tao 1 muc moi va luu vao
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictianory.Add(_item, newItem);
        }

        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))//Tim item trong InventoryItem neu co thi true
        {
            if (value.stackSize <= 1)//Kiem tra so luong item <= 1 thi xoa khoi danh sach inventoryItems va loai bo khoi tu dien inventoryDictionary
            {
                inventoryItems.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else//Khong thi tru so luong
                value.RemoveStack();
        }

        UpdateSlotUI();
    }
}
