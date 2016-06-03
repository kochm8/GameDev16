using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnitySteer.Behaviors;

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
        if(this.canSpawn())
        {
            //Debug.Log("Spawn Entity!");
            this.spawnEntity();
        }
        GameObject efb = transform.GetChild(0).gameObject;
        GameObject model = efb.transform.GetChild(0).gameObject;
        //Debug.Log("EFB:");
        //Debug.Log(efb.transform.localPosition);
        //Debug.Log("Model:");


    }

    protected virtual void spawnEntity()
    {
        //increase counter
        this.increaseCounter();
        Debug.Log("PARENT");
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
        return this.getCounter() < numEntities ? true : false;
    }


}