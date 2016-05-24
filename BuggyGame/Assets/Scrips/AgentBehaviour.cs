using UnityEngine;
using System.Collections;
using UnitySteer.Behaviors;

public class AgentBehaviour : MonoBehaviour {
    
    public int removeDistance = 40;
    private float underwaterLevel;
    private GameObject buggy;
    private GameObject terrain;

    void Start () {
        buggy = GameObject.Find("buggy");
        terrain = GameObject.Find("Terrain");
        underwaterLevel = Underwater.underwaterLevel;
        underwaterLevel -= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //delete fish if out of range
        float distanceToBuggy = Vector3.Distance(buggy.transform.position, transform.position);
        if (distanceToBuggy > removeDistance)
        {
            destroyFish();
        }

        //delete fish if under terrain
        if (transform.position.y < Terrain.activeTerrain.SampleHeight(transform.position))
        {
            destroyFish();
        }

        //fix fish underwater Y-Position
        if (transform.position.y > underwaterLevel)
        {
            transform.position = new Vector3(transform.position.x, underwaterLevel, transform.position.z);
        }
    }

    void destroyFish()
    {
        //decrease Counter
        FishCount fc = FishCount.Instance;
        fc.decreaseCounter();

        //delete fish
        Destroy(gameObject);
    }

    void fixedUpdate()
    {
    }
}
