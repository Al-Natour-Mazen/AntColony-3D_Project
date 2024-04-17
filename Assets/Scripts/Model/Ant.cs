/// <summary>
/// Ant management class
/// </summary>
public class Ant
{
    private static int TAKE_PROB = 3;
    private static double DEPOSIT_PROB = 20;
    private int x, y;
    private bool carryingSeed;
    private int colonyX;
    private int colonyY;

    /// <summary>
    /// Creates an ant
    /// </summary>
    /// <param name="x">X position of the ant</param>
    /// <param name="y">Y position of the ant</param>
    /// <param name="carryingSeed">Whether the ant is carrying a seed or not</param>
    public Ant(int x, int y, bool carryingSeed, int colonyX, int colonyY)
    {
        this.x = x;
        this.y = y;
        this.carryingSeed = carryingSeed;
        this.colonyX = colonyX;
        this.colonyY = colonyY;
    }

    /// <summary>
    /// Creates an ant at (1,1) which is not carrying anything
    /// </summary>
    public Ant()
    {
        this.x = 1;
        this.y = 1;
        this.carryingSeed = false;
    }

    /// <summary>
    /// Indicates if an ant is carrying a seed.
    /// </summary>
    /// <returns>True if the ant is carrying a seed</returns>
    public bool IsCarryingSeed()
    {
        return carryingSeed;
    }

    /// <summary>
    /// The ant picks up a seed if it didn't already have one
    /// </summary>
    public void Take()
    {
        carryingSeed = true;
    }

    /// <summary>
    /// The ant drops its seed if it had one
    /// </summary>
    public void Drop()
    {
        carryingSeed = false;
    }

    // Probabilities of picking up and dropping a seed

    /// <summary>
    /// Returns a probability (value between 0.0 and 1.0)
    /// based on an integer >= 0. This probability will be used to determine if the ant picks up a seed
    /// based on the number of seeds around the ant
    /// </summary>
    /// <param name="nbSeeds">Integer</param>
    /// <returns>Probability of picking up</returns>
    public static double TakeProbability(int nbSeeds)
    {
        return (1.0 / (double)(nbSeeds + TAKE_PROB));
    }

    /// <summary>
    /// Returns a probability (value between 0.0 and 1.0)
    /// based on an integer >= 0. This probability will be used to determine if the ant drops a seed
    /// based on the number of seeds around the ant
    /// </summary>
    /// <param name="nbSeeds">Integer</param>
    /// <returns>Probability of dropping</returns>
    public static double DropProbability(int nbSeeds)
    {
        return (1.0 - (19.9 / ((double)nbSeeds + DEPOSIT_PROB)));
    }

    /// <summary>
    /// Returns the X position of the ant
    /// </summary>
    /// <returns>The X position of the ant</returns>
    public int GetX()
    {
        return x;
    }

    /// <summary>
    /// Returns the Y position of the ant
    /// </summary>
    /// <returns>The Y position of the ant</returns>
    public int GetY()
    {
        return y;
    }

    /// <summary>
    /// Sets the X position of the ant
    /// </summary>
    /// <param name="x">The X position</param>
    public void SetX(int x)
    {
        this.x = x;
    }

    /// <summary>
    /// Sets the Y position of the ant
    /// </summary>
    /// <param name="y">The Y position</param>
    public void SetY(int y)
    {
        this.y = y;
    }

    /// <summary>
    /// Gets the X position of the ant's colony.
    /// </summary>
    /// <returns>The X position of the ant's colony</returns>
    public int GetColonyX()
    {
        return colonyX;
    }

    /// <summary>
    /// Gets the Y position of the ant's colony.
    /// </summary>
    /// <returns>The Y position of the ant's colony</returns>
    public int GetColonyY()
    {
        return colonyY;
    }
}