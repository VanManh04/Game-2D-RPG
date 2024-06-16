using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;//so luong item muon drop
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefabs;
    [SerializeField] private ItemData item;

    public void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)   
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        if (dropList.Count < possibleItemDrop)
            return;
        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    public void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefabs, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5,5), Random.Range(15,20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
