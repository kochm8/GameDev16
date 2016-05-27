using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwarmInitiatior : MonoBehaviour {

    public int numEntities = 50;
    // public float speed = 50;
    public bool birds = false;
    public GameObject model;

    public int maxHeight = 4;
    private int counter=0;


    // Update is called once per frame
    void Update()
    {
        Debug.Log("SwarmInitiator:UPDATE");
        if(this.canSpawn())
        {
            this.spawnEntity();
        }
    }

    protected virtual void spawnEntity()
    {
        //increase counter
        this.increaseCounter();
        // DO SHIT
    }

    public void increaseCounter()
    {
        this.counter++;
    }

    public void decreaseCounter()
    {
        this.counter--;
    }

    public int getCounter()
    {
        return this.counter;
    }

    public virtual bool canSpawn()
    {
        Debug.Log(this.getCounter());
        return this.getCounter() < numEntities ? true : false;
    }


}