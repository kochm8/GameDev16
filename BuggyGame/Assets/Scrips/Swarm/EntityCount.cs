using System.Collections;
using UnityEngine;

public sealed class EntityCount {

    private static EntityCount instance = null;
    private int counter = 0;
    private int maxFish = 50;

    private EntityCount()
    {
    }

    public static EntityCount Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EntityCount();
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
