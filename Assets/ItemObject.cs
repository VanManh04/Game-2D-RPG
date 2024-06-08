using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ItemObject: Day la 1 lop de quan ly vat pham trong tro choi
public class ItemObject : MonoBehaviour
{

    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            //Debug.Log("Piked up item " + itemData.itemName);
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
