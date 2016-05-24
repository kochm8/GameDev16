using UnityEngine;

public class Prefs {

    public float suspensionDistance;
    public float hue;
    public float saturation;
    public float value;

    public void Load()
    {
        suspensionDistance = PlayerPrefs.GetFloat("suspensionDistance", 0.2f);

        hue = PlayerPrefs.GetFloat("hue", 0.2f);
        saturation = PlayerPrefs.GetFloat("saturation", 0.7f);
        value = PlayerPrefs.GetFloat("value", 0.3f);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("suspensionDistance", suspensionDistance);

        PlayerPrefs.SetFloat("hue", hue);
        Debug.Log("save hue:"+hue);
        PlayerPrefs.SetFloat("saturation", saturation);
        PlayerPrefs.SetFloat("value", value);
    }
    public void SetAll(ref WheelCollider wheelFL, ref WheelCollider wheelFR,
                       ref WheelCollider wheelRL, ref WheelCollider wheelRR, 
                       ref GameObject buggyBody)
    {
        SetWheelColliderSuspension(ref wheelFL, ref wheelFR,
                                    ref wheelRL, ref wheelRR);
        SetBuggyBodyColor(ref buggyBody);
    }

    public void SetWheelColliderSuspension(ref WheelCollider wheelFL,
                                            ref WheelCollider wheelFR,
                                           ref WheelCollider wheelRL,
                                            ref WheelCollider wheelRR)
    {
        wheelFL.suspensionDistance = suspensionDistance;
        wheelFR.suspensionDistance = suspensionDistance;
        wheelRL.suspensionDistance = suspensionDistance;
        wheelRR.suspensionDistance = suspensionDistance;
    }

    public void SetBuggyBodyColor(ref GameObject buggyBody)
    {
        
        var buggyMaterial = buggyBody.GetComponent<Renderer>().material;
        buggyMaterial.color = Color.HSVToRGB(hue, saturation, value);
        
        /*
        var buggyMaterial = GameObject.Find("body").GetComponent<Renderer>().material;
        buggyMaterial.color = Color.HSVToRGB(hue, saturation, value);
        Debug.Log("set hue:" + hue);
        */
    }
}
