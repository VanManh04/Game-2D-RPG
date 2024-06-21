using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        ui.itemTooltip.HideToopTip();

        //Debug.Log("Equiped new item: " + item.data.itemName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Show item info " + item.data.itemName);
        if (item == null)
            return;

        Vector2 mousePosition = Input.mousePosition;
        //print(mousePosition);

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 1000)
            xOffset = -250;
        else
            xOffset = 250;

        if (mousePosition.y > 579)
            yOffset = -50;
        else
            yOffset = 200;

        ui.itemTooltip.ShowToolTip(item.data as ItemData_Equipment);

        ui.itemTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        //Debug.Log("Hide item info " + item.data.itemName);
        ui.itemTooltip.HideToopTip();
    }

}
