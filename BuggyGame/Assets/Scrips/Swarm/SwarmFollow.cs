using UnityEngine;
using System.Collections;
using UnitySteer.Behaviors;

public class SwarmFollow : SwarmInitiatior {
    public GameObject target;
    public float maximumDistance = 50;
    protected string entity_behaviour = "Swarm/Behaviours/EntityFollowBehaviour";

    protected override void spawnEntity()
    {
        this.increaseCounter();
        //calc entity Position
        float offset = 0.2f;
        float distance = 30.0f;
        Transform targetTransform = target.GetComponent<Transform>();
        Vector3 entityPosition = targetTransform.position + targetTransform.forward * distance;
        entityPosition.y = Random.Range(offset, maxHeight - offset);
        entityPosition.x += Random.Range(-10, 10);
        entityPosition.z += Random.Range(0, 10);

        //spwan entity
        GameObject entity = (GameObject)Instantiate(Resources.Load(this.entity_behaviour));
        // entity.transform.parent = this.transform;
        entity.GetComponent<SteerForEvasion>().Menace = target.GetComponent<AutonomousVehicle>();
        entity.GetComponent<SteerToFollow>().Target = target.transform;
        entity.transform.position = entityPosition;
        GameObject go = (GameObject)Instantiate(this.model);
        go.transform.parent = entity.transform;
    }

    public override bool canSpawn()
    {
        Debug.Log("SwarmFollow.canSpawn");

        return base.canSpawn() && target.transform.position.y < maxHeight;
    }

}
