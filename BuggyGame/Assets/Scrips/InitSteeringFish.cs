using UnityEngine;
using System.Collections;
using UnitySteer.Behaviors;

public class InitSteeringFish : MonoBehaviour {

    private int underwaterLevel = 4;
    private GameObject buggy;

    void Start () {
        buggy = GameObject.Find("buggy");
    }
	
	void Update () {

        //spawn of buggy under water
        if (transform.position.y < underwaterLevel)
        {
            EntityCount fc = EntityCount.Instance;
            if (fc.canSpawn())
            {
                spawnFish();
            }
        }
    }


    public void spawnFish()
    {
        //increase counter
        EntityCount fc = EntityCount.Instance;
        fc.increaseCounter();

        //calc fish Position
        float offset = 0.2f;
        float distance = 30.0f;
        Transform buggyTransform = buggy.GetComponent<Transform>();
        Vector3 fishPosition = buggyTransform.position + buggyTransform.forward * distance;
        fishPosition.y = Random.Range(offset, underwaterLevel - offset);
        fishPosition.x += Random.Range(-10, 10);
        fishPosition.z += Random.Range(0, 10);

        //spwan fish
        GameObject fish = (GameObject)Instantiate(Resources.Load("SteeringFish"));
        fish.GetComponent<SteerForEvasion>().Menace = buggy.GetComponent<AutonomousVehicle>();
        fish.GetComponent<SteerToFollow>().Target = buggy.transform;
        //fish.GetComponent<AgentBehaviour>().buggy = buggy;
        fish.transform.position = fishPosition;
    }
}
