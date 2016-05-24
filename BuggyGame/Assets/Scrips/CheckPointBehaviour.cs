using UnityEngine;
using System.Collections;

public class CheckPointBehaviour : MonoBehaviour {

	void Start () {	}

	void Update () { }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            gameObject.SetActive(false);
            ArrowBehaviour.nextCheckpoint();
        }
    }
}