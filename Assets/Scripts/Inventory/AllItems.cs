using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    [SerializeField] List<GameObject> itemPrefabs;

    private List<Item> items;

    public List<Item> GetAllItems()
    {
        if (items == null)
        {
            items = new List<Item>(itemPrefabs.Count);
            foreach (GameObject prefab in itemPrefabs)
            {
                items.Add(prefab.GetComponent<Item>());
            }
        }
        return items;
    }
}
