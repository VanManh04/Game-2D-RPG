using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//ItemObject: Day la 1 lop de quan ly vat pham trong tro choi
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PuckupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
