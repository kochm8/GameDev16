using UnityEngine;

public sealed class FishCount {

    private static FishCount instance = null;
    private int counter = 0;
    private int maxFish = 50;

    private FishCount()
    {
    }

    public static FishCount Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FishCount();
            }
            return instance;
        }
    }

    public void increaseCounter()
    {
        counter++;
    }

    public void decreaseCounter()
    {
        counter--;
    }

    public bool canSpawn()
    {
        return counter < maxFish ? true : false;
    }


}
