using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

    public Transform buggy;
    public GameObject checkPoint0;
    public GameObject checkPoint1;
    public GameObject checkPoint2;
    public GameObject checkPoint3;
    public GameObject checkPoint4;
    public GameObject checkPoint5;
    public GameObject checkPoint6;
    public GameObject checkPoint7;
    public GameObject checkPoint8;
    public GameObject checkPoint9;
    public GameObject checkPoint10;
    public GameObject checkPoint11;

    private float distance = 4.0f;
    private Transform lookAtCheckPoint;
    private ArrayList list = new ArrayList();
    private static int checkpointId = 0;

    void Start()
    {
        list.Add(checkPoint0);
        list.Add(checkPoint1);
        list.Add(checkPoint2);
        list.Add(checkPoint3);
        list.Add(checkPoint4);
        list.Add(checkPoint5);
        list.Add(checkPoint6);
        list.Add(checkPoint7);
        list.Add(checkPoint8);
        list.Add(checkPoint9);
        list.Add(checkPoint10);
        list.Add(checkPoint11);


        for (int i = 0; i < list.Count; i++)
        {
            GameObject currentCheckpoint = (GameObject)list[i];
            currentCheckpoint.SetActive(false);
        }
    }


    void FixedUpdate()
    {
        transform.position = new Vector3(buggy.position.x, buggy.position.y + distance, buggy.position.z);
        if (checkpointId < list.Count)
        {
            GameObject currentCheckpoint = (GameObject)list[checkpointId];
            currentCheckpoint.SetActive(true);
            currentCheckpoint.GetComponent<Renderer>().enabled = true;
            transform.LookAt(currentCheckpoint.transform);
            transform.Rotate(new Vector3(1, 0, 0), 270);
        }
    }

    public static void nextCheckpoint()
    {
        checkpointId++;
    }
}
