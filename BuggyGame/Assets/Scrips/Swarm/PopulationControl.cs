using UnityEngine;
using System.Collections;
using UnitySteer.Behaviors;

public class PopulationControl : MonoBehaviour {
    
    private float removeDistance;
    private float maxHeight;
    private GameObject target;
    private Terrain terrain;
    private int groupID;
    private SwarmFollow swarmFollow;

    void Start () {
        this.swarmFollow = this.transform.parent.GetComponent<SwarmFollow>();
        this.terrain = Terrain.activeTerrain;
        this.maxHeight = this.swarmFollow.maxHeight;
        this.removeDistance = this.swarmFollow.maximumDistance;
        this.target = this.swarmFollow.target;
        this.groupID = this.swarmFollow.GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        //delete fish if out of range
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget > removeDistance)
        {
            destroyEntity();
        }

        //delete fish if under terrain
        if (Terrain.activeTerrain && transform.position.y < Terrain.activeTerrain.SampleHeight(transform.position))
        {
            destroyEntity();
        }

        //fix fish underwater Y-Position
        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }
    }

    void destroyEntity()
    {
        this.swarmFollow.decreaseCounter();

        //delete fish
        Destroy(gameObject);
    }

}
