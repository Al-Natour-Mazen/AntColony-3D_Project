
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AntColony
{
    private HashSet<Ant> theAnts;
    private int xCoordinateColony, yCoordinateColony;
    private int seedQuantity = 0;
    private GridColony gridColony;

    public AntColony(int xCoordinateColony, int yCoordinateColony, GridColony gridGame)
    {
        theAnts = new HashSet<Ant>();
        this.xCoordinateColony = xCoordinateColony;
        this.yCoordinateColony = yCoordinateColony;
        gridColony = gridGame;
    }

    public int GetXColonyCoordinate()
    {
        return xCoordinateColony;
    }

    public int GetYColonyCoordinate()
    {
        return yCoordinateColony;
    }

    public int GetSeedQuantity()
    {
        return seedQuantity;
    }

    public void SetSeedQuantity(int quanity)
    {
        seedQuantity += quanity;
    }

    public HashSet<Ant> GetTheAnts()
    {
        return theAnts;
    }

    public void Progress()
    {
        List<Ant> antsToRemove = new List<Ant>();
        List<Ant> ItFourmi = theAnts.ToList();
        foreach (Ant f in ItFourmi)
        {
            int posX = f.GetX();
            int posY = f.GetY();
            if (!f.IsCarryingSeed() && gridColony.GetSeedQuantity(f.GetX(), f.GetY())> 0)
            {
                if (UnityEngine.Random.value < Ant.TakeProbability(gridColony.CountSeedsNeighbors(posX, posY)))
                {
                    f.Take();
                    gridColony.SetSeedQuantity(posX, posY, gridColony.GetSeedQuantity(posX, posY) -1);
                }
            }
            int deltaX;
            int deltaY;
            int cptEssai = 0;
            do
            {
                cptEssai++;
                deltaX = posX;
                deltaY = posY;
                int tirage = UnityEngine.Random.Range(0, 8);
                (deltaX, deltaY) = UpdateCoordinates(tirage, deltaX, deltaY);
            } while ((gridColony.GetWall(deltaX, deltaY) || gridColony.ContainsAnt(deltaX, deltaY) ) && cptEssai < 100);
            if (cptEssai < 99)
            {
                UpdateAntsArray(posX, posY, deltaX, deltaY, f);
                if (f.IsCarryingSeed() && (f.GetX() != f.GetColonyX() || f.GetY() != f.GetColonyY()))
                {
                    HandleAntCarryingSeed(f, antsToRemove);
                }
            }
        }
        foreach (Ant fourmi in antsToRemove)
        {
            // Ajouter une nouvelle fourmi à la colonie avec des coordonnées aléatoires
            int randomX = UnityEngine.Random.Range(1, gridColony.GetWidth() + 1);
            int randomY = UnityEngine.Random.Range(1, gridColony.GetHeight() + 1);
            this.AddNewAnt(randomX, randomY);
            theAnts.Remove(fourmi);
        }
    }

    private (int, int) UpdateCoordinates(int tirage, int deltaX, int deltaY)
    {
        switch (tirage)
        {
            case 0:
                deltaX--;
                deltaY--;
                break;
            case 1:
                deltaY--;
                break;
            case 2:
                deltaX++;
                deltaY--;
                break;
            case 3:
                deltaX--;
                break;
            case 4:
                deltaX++;
                break;
            case 5:
                deltaX--;
                deltaY++;
                break;
            case 6:
                deltaY++;
                break;
            case 7:
                deltaX++;
                deltaY++;
                break;
        }
        return (deltaX, deltaY);
    }

    private void UpdateAntsArray(int posX, int posY, int deltaX, int deltaY, Ant f)
    {
        gridColony.SetAnt(posX, posY,false);
        gridColony.SetAnt(deltaX, deltaY, true);
        f.SetX(deltaX);
        f.SetY(deltaY);
    }

    private void HandleAntCarryingSeed(Ant f, List<Ant> aSupprimer)
    {
        int deltaXX = f.GetColonyX() - f.GetX();
        int deltaYY = f.GetColonyY() - f.GetY();
        if (deltaXX != 0)
            deltaXX /= Mathf.Abs(deltaXX);
        if (deltaYY != 0)
            deltaYY /= Mathf.Abs(deltaYY);
        int newPosX = f.GetX() + deltaXX;
        int newPosY = f.GetY() + deltaYY;
        if (!gridColony.GetWall(newPosX, newPosY)  && !gridColony.ContainsAnt(newPosX, newPosY))
        {
            gridColony.SetAnt(f.GetX(), f.GetY(), false);
            gridColony.SetAnt(newPosX, newPosY, true);
            f.SetX(newPosX);
            f.SetY(newPosY);
            return;
        }
        if (newPosX == f.GetColonyX() && newPosY == f.GetColonyY())
        {
            f.Drop();
            seedQuantity++;
            gridColony.SetSeedQuantity(f.GetX(), f.GetY(), 0);
            gridColony.SetAnt(f.GetX(), f.GetY(), false);
            aSupprimer.Add(f);
        }
    }

    public void AddNewAnt(int x, int y)
    {
        Ant ant;
        if (!gridColony.ContainsAnt(x,y) && !gridColony.GetWall(x,y))              
        {
            ant = new Ant(x, y, false, xCoordinateColony, yCoordinateColony);
            gridColony.SetAnt(x, y, true);
        }
        else
        {
            //on spawn la fourmi à la fourmiliere
            ant = new Ant(xCoordinateColony, yCoordinateColony, false, xCoordinateColony, yCoordinateColony);
            gridColony.SetAnt(xCoordinateColony, yCoordinateColony, true);
        }
        theAnts.Add(ant);
    }

}

