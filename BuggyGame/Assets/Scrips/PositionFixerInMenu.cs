using UnityEngine;
using System.Collections;

public class PositionFixerInMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.position = new Vector3(0.0f, transform.position.y, 0.0f);
    }
}
