using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {

    private float moveSpeed = 5;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        var vector3Move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(vector3Move * moveSpeed * Time.deltaTime, Space.World);
    }
}
