using UnityEngine;
using System.Collections;
using UnitySteer.Behaviors;

public class SwarmFollow : SwarmInitiatior {
    public GameObject target;
    public float maximumDistance = 50;
    public float spawnDistance = 10;
    public float bigPercentage = 0.1f;
    public float minScaleFactor = 0.1f;
    public float maxScaleFactor = 0.75f;
    protected string entity_behaviour = "Swarm/Behaviours/EntityFollowBehaviour";

    protected override void spawnEntity()
    {
        this.increaseCounter();
        //calc entity Position
        float offset = 0.2f;
        float distance = this.spawnDistance;
        Transform targetTransform = target.GetComponent<Transform>();
        Vector3 entityPosition = targetTransform.position + targetTransform.forward * distance;
        entityPosition.y = Random.Range(offset, maxHeight - offset);
        entityPosition.x += Random.Range(-10, 10);
        entityPosition.z += Random.Range(0, 10);

        float min = Random.Range(0.0f, 1.0f) <= this.bigPercentage ? this.maxScaleFactor - this.minScaleFactor : this.minScaleFactor;
        float size = Random.Range(min, this.maxScaleFactor);
        Debug.Log(size);
        //spwan entity
        GameObject swarm_behaviour = (GameObject)Instantiate(Resources.Load(this.entity_behaviour));
        // entity.transform.parent = this.transform;
        // swarm_behaviour.GetComponent<SteerForEvasion>().Menace = target.GetComponent<PassiveVehicle>();
        swarm_behaviour.GetComponent<SteerForPursuit>().Quarry = target.GetComponent<DetectableObject>();
        swarm_behaviour.GetComponent<SteerToFollow>().Target = target.transform;
        swarm_behaviour.transform.position = entityPosition;
        swarm_behaviour.transform.parent = this.transform;

        GameObject entity_model = (GameObject)Instantiate(this.model);
        entity_model.transform.parent = swarm_behaviour.transform;
        entity_model.transform.position = new Vector3(0, 0, 0);
        entity_model.transform.localPosition = new Vector3(0, 0, 0);
        Vector3 prev = entity_model.transform.localScale;
        entity_model.transform.localScale = new Vector3(prev.x * size, prev.y * size, prev.z * size);

    }

    public override bool canSpawn()
    {

        return base.canSpawn() && target.transform.position.y < maxHeight;
    }

}
