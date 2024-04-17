using System.Collections.Generic;
using UnityEngine.UIElements.Experimental;

public class GridColony
{
    private bool[,] walls;
    private bool[,] ants;
    private int[,] seedQuantities;
    private int width, height;
    private int maxQuantityOnBlock;

    public GridColony(int width, int height, int MaxQuantitySeedPerBlock)
    {
        this.width = width;
        this.height = height;
        this.maxQuantityOnBlock = MaxQuantitySeedPerBlock;

        ants = new bool[width + 2, height + 2];
        walls = new bool[width + 2, height + 2];
        seedQuantities = new int[width + 2, height + 2];

        for (int i = 0; i < width + 2; i++)
        {
            for (int j = 0; j < height + 2; j++)
            {
                ants[i, j] = false;
                walls[i, j] = (i == 0) || (i == width + 1) || (j == 0) || (j == height + 1);
                seedQuantities[i, j] = 0;
            }
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public bool GetWall(int x, int y)
    {
        return walls[x, y];
    }

    public void SetWall(int x, int y, bool isWallPresent)
    {
        if (x > 0 && x < width + 1 && y > 0 && y < height + 1 && x != 0 && y != 0)
            walls[x, y] = isWallPresent;
    }

    public bool ContainsAnt(int x, int y)
    {
        return ants[x, y];
    }

    public void SetAnt(int x, int y, bool isAntPresent)
    {
        ants[x, y] = isAntPresent;
    }

    public int GetSeedQuantity(int x, int y)
    {
        return seedQuantities[x, y];
    }

    public void SetSeedQuantity(int x, int y, int quantity)
    {
        if (quantity < 0 || quantity > maxQuantityOnBlock)
            return;

        seedQuantities[x, y] = quantity;
    }

    public int CountTotalSeeds()
    {
        int totalSeeds = 0;

        // Traverse each cell in the grid
        for (int x = 0; x < width + 2; x++)
        {
            for (int y = 0; y < height + 2; y++)
            {
                // Add the seed quantity in the cell to totalSeeds
                totalSeeds += seedQuantities[x, y];
            }
        }

        return totalSeeds;
    }

    public int CountWalls()
    {
        int totalWalls = 0;

        // Traverse each cell in the grid
        for (int x = 0; x < width + 2; x++)
        {
            for (int y = 0; y < height + 2; y++)
            {
                // If the cell is a wall, increment the wall counter
                if (walls[x, y])
                {
                    totalWalls++;
                }
            }
        }

        return totalWalls;
    }

    public int CountSeedsNeighbors(int x, int y)
    {
        int nb = 0;
        for (int vx = -1; vx < 2; vx++)
        {
            for (int vy = -1; vy < 2; vy++)
            {
                if (!walls[x + vx, y + vy])
                {
                    nb += seedQuantities[x + vx, y + vy];
                }
            }
        }
        return nb;
    }

    public int getMaxQuantityOnBlock()
    {
        return maxQuantityOnBlock;
    }
}