using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public Slider sliSuspDistance;
    public Text txtDistanceNum;
    private Prefs _prefs;

    public GameObject buggyBody;
    public Text txtHue;
    public Text txtSaturation;
    public Text txtValue;
    private Material buggyMaterial;

    private float _hue = 0.2f;
    private float _saturation = 0.7f;
    private float _value = 0.3f;


    void Start()
    {
        buggyMaterial = buggyBody.GetComponent<Renderer>().material;

        _prefs = new Prefs();
        _prefs.Load();
        _prefs.SetAll(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR, ref buggyBody);

        sliSuspDistance.value = _prefs.suspensionDistance;
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");

    }

    public void OnSliderChangedSuspDistance(float suspDistance)
    {
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");
        _prefs.suspensionDistance = suspDistance;
        _prefs.SetWheelColliderSuspension(ref wheelFL, ref wheelFR,
                                          ref wheelRL, ref wheelRR);
    }

    public void OnSliderChangedHue(float hue)
    {
        txtHue.text = hue.ToString("0.00");
        _prefs.hue = hue;
        _prefs.SetBuggyBodyColor(ref buggyBody);
    }

    public void OnSliderChangedSaturation(float saturation)
    {
        txtSaturation.text = saturation.ToString("0.00");
        _prefs.saturation = saturation;
        _prefs.SetBuggyBodyColor(ref buggyBody);
    }

    public void OnSliderChangedValue(float value)
    {
        txtValue.text = value.ToString("0.00");
        _prefs.value = value;
        _prefs.SetBuggyBodyColor(ref buggyBody);
    }

    public void setBuggyColor()
    {
        buggyMaterial.color = Color.HSVToRGB(_hue, _saturation, _value);
    }


    public void OnButtonClickStart()
    {
        _prefs.Save();
        SceneManager.LoadScene("Scene1");
    }

    void OnApplicationQuit()
    {
        _prefs.Save();
    }
}