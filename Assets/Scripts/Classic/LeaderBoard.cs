using System.Collections.Generic;
using UnityEngine;
using static AntColonyPersistenceManager;
using static ContentScrollExpandableItem;

public class Leaderboard : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Reference to the ContentScrollExpandableItem component.")]
    public ContentScrollExpandableItem contentItem;

    [Tooltip("Reference to the GameObject representing the leaderboard panel.")]
    public GameObject leaderboardPanel;

    private bool isPanelActive = false;

    void Start()
    {
        // Ensure leaderboard panel is in the correct initial state
        SetLeaderboardPanelActive(isPanelActive);
    }

    // Toggle the visibility of the leaderboard panel
    public void ToggleLeaderboardPanel()
    {
        isPanelActive = !isPanelActive;
        SetLeaderboardPanelActive(isPanelActive);
        if (isPanelActive)
        {
            // Load leaderboard information if panel is active
            LoadLeaderboardInfo();
        }
    }

    // Set the active state of the leaderboard panel
    void SetLeaderboardPanelActive(bool isActive)
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(isActive);
        }
    }

    // Load leaderboard information
    void LoadLeaderboardInfo()
    {
        if (leaderboardPanel != null)
        {
            // Load saved colony information from persistence manager
            List<antColInfos> savedColonyList = AntColonyPersistenceManager.LoadColonyInfo();
            if (savedColonyList != null)
            {
                int savedColonyCount = savedColonyList.Count;
                List<ItemInfo> items = new List<ItemInfo>(savedColonyCount);
                for (int i = 0; i < savedColonyCount; i++)
                {
                    ItemInfo infoItem = new ItemInfo();

                    infoItem.title = "Ant Simulation id :";
                    infoItem.titleBis = savedColonyList[i].number.ToString();

                    string colonyInfo = "";
                    colonyInfo += "Dimension: " + savedColonyList[i].width + "x" + savedColonyList[i].height + "\n";
                    colonyInfo += "Ant Colony Coordinate: " + savedColonyList[i].X + "," + savedColonyList[i].Y + ",Gap Around the Colony " + savedColonyList[i].GapAroundHill + "\n";
                    colonyInfo += "Number of Ants: " + savedColonyList[i].NBAnts + "\n";
                    colonyInfo += "Total Seeds: in Colony " + savedColonyList[i].SeedsInColony + " / out Colony " + savedColonyList[i].SeedsOutColony + "\n";
                    colonyInfo += "Max Seed Quantity on Block: " + savedColonyList[i].MaxSeedBlock + "\n";

                    infoItem.description = colonyInfo;

                    items.Add(infoItem);
                }
                // Set the list of items in the content scroll
                contentItem.SetListItem(items);
            }
        }
    }
}
