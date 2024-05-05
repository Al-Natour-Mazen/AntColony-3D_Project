using System.Collections.Generic;
using UnityEngine;

public class ContentScrollExpandableItem : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("The prefab to instantiate for each item")]
    public ExpandableItem itemPrefab; // The prefab to spawn

    [Header("Item Holder")]
    [Tooltip("The transform where instantiated items will be parented")]
    public Transform itemHolder;

    // Structure to hold information about each item
    public struct ItemInfo
    {
        public string title;         // Main title of the item
        public string titleBis;      // Subtitle of the item
        public string description;   // Description of the item
    }

    // Method to set the list of items
    public void SetListItem(List<ItemInfo> itemList)
    {
        // Clear the holder before instantiating new items
        foreach (Transform child in itemHolder)
        {
            Destroy(child.gameObject);
        }

        int itemCount = itemList.Count;
        for (int i = 0; i < itemCount; i++)
        {
            // Instantiate the prefab and add it to the list
            ExpandableItem newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, itemHolder);

            // Get information for the current item
            string mainTitle = itemList[i].title;
            string subTitle = itemList[i].titleBis;
            string description = itemList[i].description;

            // Assign the information to the corresponding prefab
            newItem.SetTitle(mainTitle);
            newItem.SetTitleBis(subTitle);
            newItem.SetDescription(description);
        }
    }
}
