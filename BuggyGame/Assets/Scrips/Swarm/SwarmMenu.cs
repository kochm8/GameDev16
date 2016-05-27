using UnityEditor;
using UnityEngine;
using System.Collections;

public class SwarmMenu : MonoBehaviour {
    static string swarm_prefab = "Swarm/Swarm";
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("GameObject/SwarmFollow")]
    static void CreateSwarmObject()
    {
        Debug.Log(SwarmMenu.swarm_prefab);
        GameObject go = (GameObject)Resources.Load(SwarmMenu.swarm_prefab, typeof(GameObject));
        Debug.Log(go);
        
        GameObject swarm = GameObject.Instantiate(go);
        swarm.transform.parent = Selection.activeGameObject.transform;
        swarm.name = "Swarm";
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
