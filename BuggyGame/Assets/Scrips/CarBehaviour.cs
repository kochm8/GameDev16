using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CarBehaviour : MonoBehaviour {

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public Transform centerOfMass;
    private Rigidbody _rigidBody;

    public float maxTorque = 500;
    public float maxSteerAngle = 40;
    public float maxSpeedKMH = 120;
    public float maxSpeedBackwardKMH = 30;
    private float _currentSpeedKMH;

    public float maxBrakeTorque = 500;
    public float fullBrakeTorque = 3000;

    public float forewardFriction = 5;
    public float sidewaysFriction = 5;

    //public GUIText guiSpeed;
    public Texture2D guiArrow;
    public Texture2D guiSpeedDisplay;
    public Texture2D guiSpeedPointer;
    public GUISkin guiSkin;

    public AudioClip engineSingleRPMSoundClip;
    private AudioSource _engineAudioSource;

    public AudioClip brakeAudioClip;
    private AudioSource _brakeAudioSource;

    private ParticleSystem.EmissionModule _smokeLEmission;
    private ParticleSystem.EmissionModule _smokeREmission;

    private ParticleSystem.EmissionModule _dustRLEmission;
    private ParticleSystem.EmissionModule _dustRREmission;
    private ParticleSystem.EmissionModule _dustFLEmission;
    private ParticleSystem.EmissionModule _dustFREmission;

    private ParticleSystem.EmissionModule _bubbleLEmission;
    private ParticleSystem.EmissionModule _bubbleREmission;

    public bool thrustEnabled = false;

    public GameObject buggyBody;

    private Scene _scene;
    private Prefs _prefs;

    private bool _carIsOnDrySand;
    private bool _carIsNotOnSand;
    private String _groundTag;
    private int _groundTexture;

    // Use this for initialization
    void Start () {

        //var buggyMaterial = GameObject.Find("body").GetComponent<Renderer>().material;
        //buggyMaterial.color = Color.HSVToRGB(200,0.5f,200);

        _scene = SceneManager.GetActiveScene();
        //guiSpeed.color = Color.red;

        _prefs = new Prefs();
        _prefs.Load();
        _prefs.SetAll(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR, ref buggyBody);

        /*
        Vector3 guiSpeedPos = guiSpeed.transform.position;
        guiSpeedPos.z = 2.0f;
        guiSpeed.transform.position = guiSpeedPos;
        */

        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = new Vector3(centerOfMass.localPosition.x, centerOfMass.localPosition.y, centerOfMass.localPosition.z);

        SetFriction(forewardFriction, sidewaysFriction);

        // Configure AudioSource component by program
        _engineAudioSource = (AudioSource)gameObject.AddComponent<AudioSource>();
        _engineAudioSource.clip = engineSingleRPMSoundClip;
        _engineAudioSource.loop = true;
        _engineAudioSource.volume = 0.7f;
        _engineAudioSource.playOnAwake = true;
        _engineAudioSource.Play();


        _smokeLEmission = GameObject.Find("SmokeL").GetComponent<ParticleSystem>().emission;
        _smokeREmission = GameObject.Find("SmokeR").GetComponent<ParticleSystem>().emission;
        _smokeLEmission.enabled = true;
        _smokeREmission.enabled = true;


        _dustRLEmission = GameObject.Find("DustRL").GetComponent<ParticleSystem>().emission;
        _dustRREmission = GameObject.Find("DustRR").GetComponent<ParticleSystem>().emission;
        _dustFLEmission = GameObject.Find("DustFL").GetComponent<ParticleSystem>().emission;
        _dustFREmission = GameObject.Find("DustFR").GetComponent<ParticleSystem>().emission;
        _dustRLEmission.enabled = true;
        _dustRREmission.enabled = true;
        _dustFLEmission.enabled = true;
        _dustFREmission.enabled = true;

        _bubbleLEmission = GameObject.Find("BubbleL").GetComponent<ParticleSystem>().emission;
        _bubbleLEmission.enabled = false;
        _bubbleREmission = GameObject.Find("BubbleR").GetComponent<ParticleSystem>().emission;
        _bubbleREmission.enabled = false;


        // Brake Audio Source
        _brakeAudioSource = gameObject.AddComponent<AudioSource>();
        _brakeAudioSource.clip = brakeAudioClip;
        _brakeAudioSource.loop = true;
        _brakeAudioSource.volume = 0.7f;
        _brakeAudioSource.playOnAwake = true;
        _brakeAudioSource.Play();
    }

    // Update is called once per frame
    void Update () {

        if (_scene.name == "Scene1")
        {
            GameObject.Find("CurrentSpeed").GetComponent<GUIText>().text = _currentSpeedKMH.ToString("0") + " km/h";
        }

    }


    // Update is called once per frame constanc time per frame
    void FixedUpdate()
    {

        // Evaluate ground
        GetGroundTagAndTextureIndex(ref _groundTag, ref _groundTexture);

        _carIsOnDrySand = _groundTag.CompareTo("Terrain") == 0 && _groundTexture == 0;
        _carIsNotOnSand = !(_groundTag.CompareTo("Terrain") == 0 && (_groundTexture <= 0));

        //Debug.Log("_carIsOnDrySand"+ _carIsOnDrySand);
        //Debug.Log("_carIsNotOnSand" + _carIsNotOnSand);

        _currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;

        //guiSpeed.text = _currentSpeedKMH.ToString("0");

        // Determine if the car is driving forwards or backwards
        bool velocityIsForeward = Vector3.Angle(transform.forward, _rigidBody.velocity) < 50f;

        /*
        / Bremsen
        */
        // Determine if the cursor key input means braking
        bool doBraking = _currentSpeedKMH > 0.5f &&
            (Input.GetAxis("Vertical") < 0 && velocityIsForeward ||
             Input.GetAxis("Vertical") > 0 && !velocityIsForeward);
        bool doFullBrake = Input.GetKey("space");

        if (doBraking || doFullBrake || !thrustEnabled)
        {

            float brakeTorque = doFullBrake ? fullBrakeTorque : maxBrakeTorque;
            if (doFullBrake && _currentSpeedKMH > 5.0f)// && _carIsNotOnSand)
            {
                _brakeAudioSource.volume = _currentSpeedKMH / 100.0f;
                _brakeAudioSource.Play();
            }

            wheelFL.brakeTorque = brakeTorque;
            wheelFR.brakeTorque = brakeTorque;
            wheelRL.brakeTorque = brakeTorque;
            wheelRR.brakeTorque = brakeTorque;
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
        else
        {
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
            wheelFL.motorTorque = maxTorque * Input.GetAxis("Vertical") * 2 ; //Drehmoment
            wheelFR.motorTorque = wheelFL.motorTorque;
        }

        if (_brakeAudioSource.isPlaying && !doFullBrake)
        {
            _brakeAudioSource.Stop();
        }
        /*
        / Geschwindigkeitslimite
        */
        float speedLimit;
        if (velocityIsForeward)
        {
            speedLimit = maxSpeedKMH; //Forward
        }
        else
        {
            speedLimit = maxSpeedBackwardKMH; //Backward
        }

        if (_currentSpeedKMH > speedLimit)
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }

        /*
        / Steuereinschlag reduzieren
        */
        float steerDecreaseFactor = Math.Max(1.0f - ((_currentSpeedKMH*1.5f) / maxSpeedKMH), 0.15f);
        //steer angle wird in abhängigkeit der Geschwindigkeit skalieren
        wheelFL.steerAngle = maxSteerAngle * steerDecreaseFactor * Input.GetAxis("Horizontal"); 
        wheelFR.steerAngle = wheelFL.steerAngle;

        //Debug.Log(Input.GetAxis("Vertical"));
        //Debug.Log("Speed KMH: " + _currentSpeedKMH + " Torque:" + wheelFL.motorTorque);
        //Debug.Log("Steer Angle: " + wheelFL.steerAngle + " AngleDecreaseFactor: " + steerDecreaseFactor);

        int gearNum = 0;
        float engineRPM = kmh2rpm(_currentSpeedKMH, out gearNum);
        SetEngineSound(engineRPM);

        SetParticleSystems(engineRPM, doFullBrake);

    }

    
    void SetFriction(float forewardFriction, float sidewaysFriction)
    {
        WheelFrictionCurve f_fwWFC = wheelFL.forwardFriction;
        WheelFrictionCurve f_swWFC = wheelFL.sidewaysFriction;
        f_fwWFC.stiffness = forewardFriction;
        f_swWFC.stiffness = sidewaysFriction;

        wheelFL.forwardFriction = f_fwWFC;
        wheelFL.sidewaysFriction = f_swWFC;
        wheelFR.forwardFriction = f_fwWFC;
        wheelFR.sidewaysFriction = f_swWFC;

        wheelRL.forwardFriction = f_fwWFC;
        wheelRL.sidewaysFriction = f_swWFC;
        wheelRR.forwardFriction = f_fwWFC;
        wheelRR.sidewaysFriction = f_swWFC;
    }



    // OnGUI is called on every frame when the orthographic GUI is rendered
    void OnGUI()
    {

        if (_scene.name == "Scene1")
        {
            GUI.skin = guiSkin;

            int sh = Screen.height;
            int size = 180;
            int offN = 20; // offset of needle

            //Tacho
            GUI.Box(new Rect(0, sh - size, size, size), guiSpeedDisplay);

            //KMH Text
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 20;
            guiStyle.normal.textColor = Color.red;
            GUI.Box(new Rect(80, sh - size + 132, size, size), "<b>" + _currentSpeedKMH.ToString("0") + "</b>", guiStyle);

            //Arrow
            /*GUIUtility.RotateAroundPivot(HandleArrow(), new Vector2(100, 100));
            GUI.Box(new Rect(100, 100, 100, 100), guiArrow);
            GUIUtility.RotateAroundPivot(-HandleArrow(), new Vector2(100, 100));*/

            //Zeiger
            float degPerKMH = (324 - 36) / 120;
            GUIUtility.RotateAroundPivot(Mathf.Abs(_currentSpeedKMH) * degPerKMH + 36,
                                    new Vector2(70 + offN, sh - size + 70 + offN));

            GUI.DrawTexture(new Rect(offN, sh - size + offN, 140, 140),
                            guiSpeedPointer,
                            ScaleMode.StretchToFill);


            //HandleArrow();

        }
    }


    void SetParticleSystems(float engineRPM, bool doFullBrake)
    {
        float smokeRate = engineRPM / 40.0f;
        _smokeLEmission.rate = new ParticleSystem.MinMaxCurve(smokeRate);
        _smokeREmission.rate = new ParticleSystem.MinMaxCurve(smokeRate);
        //Debug.Log("smokeRate: " + smokeRate);

        float dustRate = 0;
        /*
        WheelHit wheelHitFL;
        wheelFL.GetGroundHit(out wheelHitFL);

        if (_currentSpeedKMH > 10.0f) // fast enough
            if (wheelHitFL.collider)
                if (wheelHitFL.collider.CompareTag("Terrain"))
                    if (TerrainSurface.GetMainTexture(transform.position) == 0)
                    dustRate = _currentSpeedKMH * 0.5f;
                    */
        if (_currentSpeedKMH > 10.0f && (_carIsOnDrySand || doFullBrake))
        {
            dustRate = _currentSpeedKMH;

        }

        _dustFLEmission.rate = new ParticleSystem.MinMaxCurve(dustRate);
        _dustFREmission.rate = new ParticleSystem.MinMaxCurve(dustRate);
        _dustRLEmission.rate = new ParticleSystem.MinMaxCurve(dustRate);
        _dustRREmission.rate = new ParticleSystem.MinMaxCurve(dustRate);


        if (transform.position.y < Underwater.underwaterLevel)
        {
            _smokeLEmission.enabled = false;
            _smokeREmission.enabled = false;
            _dustRLEmission.enabled = false;
            _dustRREmission.enabled = false;
            _dustFLEmission.enabled = false;
            _dustFREmission.enabled = false;
            _bubbleLEmission.enabled = true;
            _bubbleREmission.enabled = true;
        }
        else
        {
            _smokeLEmission.enabled = true;
            _smokeREmission.enabled = true;
            _dustRLEmission.enabled = true;
            _dustRREmission.enabled = true;
            _dustFLEmission.enabled = true;
            _dustFREmission.enabled = true;
            _bubbleLEmission.enabled = false;
            _bubbleREmission.enabled = false;
        }
    }

    // Returns the tag and main texture of the front left wheel collider
    void GetGroundTagAndTextureIndex(ref string groundTag, ref int groundTextureIndex)
    {
        // Default values
        groundTag = "InTheAir";
        groundTextureIndex = -1;
        // Query ground by ray shoot on the front left wheel collider
        WheelHit wheelHitFL;
        wheelFL.GetGroundHit(out wheelHitFL);
        // If not in the air query collider
        if (wheelHitFL.collider)
        {
            groundTag = wheelHitFL.collider.tag;
            if (wheelHitFL.collider.CompareTag("Terrain"))
                groundTextureIndex = TerrainSurface.GetMainTexture(transform.position);
        }
    }


    class gear
    {
        public gear(float minKMH, float minRPM, float maxKMH, float maxRPM)
        {
            _minRPM = minRPM;
            _minKMH = minKMH;
            _maxRPM = maxRPM;
            _maxKMH = maxKMH;
        }
        private float _minRPM;
        private float _minKMH;
        private float _maxRPM;
        private float _maxKMH;
        public bool speedFits(float kmh)
        {
            return kmh >= _minKMH && kmh <= _maxKMH;
        }
        public float interpolate(float kmh)
        {
            //Debug.Log("kmh:" + kmh + "_minRPM:" + _minRPM + "_maxRPM:" + _maxRPM + "_minKMH:" + _minKMH + "_maxKMH:" + _maxKMH);
            float delta = (kmh - _minKMH) / (_maxKMH - _minKMH);
            float ret = ((_maxRPM - _minRPM) * delta) + _minRPM;
            //Debug.Log("retRPM:" + ret);
            return ret;
        }
    }

    /// <summary>
    /// Returns engine RPM from speed in km/h
    /// For 0 km/h it returns 800 RPM
    /// </summary>
    float kmh2rpm(float kmh, out int gearNum)
    {
        gear[] gears = new gear[]
        {   new gear(  1,  900,  12, 1400),
        new gear( 12,  900,  25, 2000),
        new gear( 25, 1350,  45, 2500),
        new gear( 45, 1950,  70, 3500),
        new gear( 70, 2500, 112, 4000),
        new gear(112, 3100, 180, 5000)
        };
        for (int i = 0; i < gears.Length; ++i)
        {
            if (gears[i].speedFits(kmh))
            {
                gearNum = i + 1;
                return gears[i].interpolate(kmh);
            }
        }
        gearNum = 1;
        return 800;
    }

    void SetEngineSound(float engineRPM)
    {
        if (_engineAudioSource == null) return;
        float minRPM = 800;
        float maxRPM = 8000;
        float minPitch = 0.3f;
        float maxPitch = 3.0f;

        //Debug.Log("engineRPM:" + engineRPM);

        float delta = (engineRPM - minRPM) / (maxRPM - minRPM);
        float pitch = ((maxPitch - minPitch) * delta) + minPitch;
        _engineAudioSource.pitch = pitch;
    }

}
