using UnityEngine;
using System.Collections;

public class WheelBehaviour : MonoBehaviour
{

    public WheelCollider wheelCol;       // wheel colider object
    private SkidmarkBehaviour _skidmarks;      // skidmark script
    private int _skidmarkLast;   // index of last skidmark
    private Vector3 _skidmarkLastPos;// position of last skidmark

    void Start()
    {
        // Get skidmarks script (not available in sceneMenu)
        GameObject skidmarksGO = GameObject.Find("Skidmarks");
        if (skidmarksGO)
            _skidmarks = skidmarksGO.GetComponent<SkidmarkBehaviour>();
        _skidmarkLast = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion quat;
        Vector3 position;
        wheelCol.GetWorldPose(out position, out quat);
        transform.position = position;
        transform.rotation = quat;

        // if wheel touches the ground: place it on the ground
        WheelHit hit;
        if (wheelCol.GetGroundHit(out hit))
            DoSkidmarking(hit);

    }


    // Creates skidmarks if handbraking
    void DoSkidmarking(WheelHit hit)
    {
        float sideWaySlip = 0.3f;
        // absolute velocity at wheel in world space
        Vector3 wheelVelo = wheelCol.attachedRigidbody.GetPointVelocity(hit.point);
        //wheelCol stehts das drin
        if (Input.GetKey("space") || (hit.sidewaysSlip > sideWaySlip) || (hit.sidewaysSlip < -sideWaySlip))
        {
            if (Vector3.Distance(_skidmarkLastPos, hit.point) > 0.1f)
            {
                _skidmarkLast = _skidmarks.Add(hit.point + wheelVelo * Time.deltaTime, hit.normal, hit.force*2/10000, _skidmarkLast);
                _skidmarkLastPos = hit.point; //0.5f, dyn setzen
            }
        }
        else _skidmarkLast = -1;
    }
}


