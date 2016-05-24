using UnityEngine;
using System.Collections;
using System;

public class TimingBehaviour : MonoBehaviour
{

    public int countMax = 3;
    private int _countDown;

    public CarBehaviour _carScript;

    private float _pastTime = 0; //Aufgabe hochzählen bei isStarted = true -> isFinished = true dann stop
    private bool _isFinished = false;
    private bool _isStarted = false;

    public AudioClip _countdownSoundClip;
    public AudioClip _countdownStartSoundClip;
    private AudioSource _coutdownAudioSource;

    void Awake()
    {
        _coutdownAudioSource = (AudioSource)gameObject.AddComponent<AudioSource>();
        //_engineAudioSource.volume = 0.7f;
        //_engineAudioSource.playOnAwake = true;
    }


    // Use this for initialization
    void Start()
    {
        //print("Begin Start:" + Time.time);
        StartCoroutine(GameStart());
        //print("End Start:" + Time.time);
    }

    // GameStart CoRoutine
    IEnumerator GameStart()
    {
        //print("   Begin GameStart:" + Time.time);

        for (_countDown = countMax; _countDown > 0; _countDown--)
        {
            Countdown(_countDown.ToString(), _countdownSoundClip);
            yield return new WaitForSeconds(1);
            //print("      WaitForSeconds:" + Time.time);
        }

        Countdown("", _countdownStartSoundClip);
        //print("   End GameStart:" + Time.time);
        _carScript.thrustEnabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            if (!_isStarted)
                _isStarted = true;
            else _isFinished = true;
        }
    }

    void Update()
    {
        if (_carScript.thrustEnabled)
        {
            if (_isStarted && !_isFinished)
                _pastTime += Time.deltaTime;
            GameObject.Find("PastTime").GetComponent<GUIText>().text = _pastTime.ToString("0.0 sec");
        }
    }

    void Countdown(String countDown, AudioClip clip)
    {
        GameObject.Find("StartCountdown").GetComponent<GUIText>().text = countDown;

        _coutdownAudioSource.clip = clip;
        _coutdownAudioSource.loop = false;
        _coutdownAudioSource.Play();
    }
}
