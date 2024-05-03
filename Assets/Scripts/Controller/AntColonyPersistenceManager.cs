using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class AntColonyPersistenceManager
{
    private static string FilePath = "Colonies.txt"; // Path to the log file
    private static int colNumber = 0;

    // Struct to hold colony information
    public struct antColInfos
    {
        public string number;
        public string width;
        public string height;
        public string NBAnts; // Number of ants
        public string X;
        public string Y;
        public string SeedsInColony;
        public string SeedsOutColony;
        public string MaxSeedBlock;
        public string GapAroundHill;
    }

    // Method to log colony information to the file
    public static void SaveColonyInfo(AntConlonySimulation antColonyToSave)
    {
        StreamWriter writer = new StreamWriter(FilePath, true);
        colNumber++;

        // Write colony information to the file
        writer.WriteLine("----- Colony Info -----");
        writer.WriteLine("Colony Saved Number: " + colNumber);
        writer.WriteLine("Width: " + antColonyToSave.GetWidthSimulation());
        writer.WriteLine("Height: " + antColonyToSave.GetHeighSimulation());
        writer.WriteLine("Ant Colony Coordinate: " + antColonyToSave.GetAntColonyCoordinate());
        writer.WriteLine("Number of Ants: " + antColonyToSave.GetAntsInColony().Count);
        writer.WriteLine("Total Seeds in Colony: " + antColonyToSave.GetTotalSeedInColony());
        writer.WriteLine("Total Seeds out of Colony: " + antColonyToSave.GetTotalSeedOutColony());
        writer.WriteLine("Max Seed Quantity on Block: " + antColonyToSave.GetMaxSeedQuantityOnBlock());
        writer.WriteLine("Gap Around The Hill: " + AntConlonySimulation.GetGapAroundHill());
        writer.WriteLine("-----------------------");

        // Close the file after writing
        writer.Close();
    }

    // Method to read colony information from the log file and store them in a list
    public static List<antColInfos> LoadColonyInfo()
    {
        List<antColInfos> readedListColony = new List<antColInfos>();

        // Check if the file exists
        if (File.Exists(FilePath))
        {
            // Read each line from the file and add to the list
            StreamReader reader = new StreamReader(FilePath);
            string line;
            antColInfos currentEntry = new antColInfos();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("Colony Saved Number:"))
                {
                    currentEntry.number = line.Split(':')[1].Trim();
                }
                else if(line.StartsWith("Width:"))
                {
                    currentEntry.width = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Height:"))
                {
                    currentEntry.height = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Ant Colony Coordinate:"))
                {
                    currentEntry.X = line.Split(':')[1].Trim().Split(',')[0];
                    currentEntry.Y = line.Split(':')[1].Trim().Split(',')[1];
                }
                else if (line.StartsWith("Number of Ants:"))
                {
                    currentEntry.NBAnts = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Total Seeds in Colony:"))
                {
                    currentEntry.SeedsInColony = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Total Seeds out of Colony:"))
                {
                    currentEntry.SeedsOutColony = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Max Seed Quantity on Block:"))
                {
                    currentEntry.MaxSeedBlock = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Gap Around The Hill:"))
                {
                    currentEntry.GapAroundHill = line.Split(':')[1].Trim();
                }
                else if (line.Equals("-----------------------"))
                {
                    readedListColony.Add(currentEntry);
                    currentEntry = new antColInfos(); // Reset the structure for the next entry
                }
            }
            reader.Close();
        }
        else
        {
            Debug.LogError("Colony file not found!");
        }

        return readedListColony;
    }
}
