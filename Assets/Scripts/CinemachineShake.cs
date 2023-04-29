using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }


    private CinemachineVirtualCamera cam;
    private float shakeTimer = 0f;
    private float shakeTimerTotal = 0f;
    private float startingIntensity = 0f;
    private bool endSmoothly = false;

    private void Awake()
    {
        Instance = this;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time, bool endSmoothly = false)
    {
        Debug.Log("Added shake");
        CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        noise.m_AmplitudeGain = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;

        startingIntensity = intensity;

        this.endSmoothly = endSmoothly;


    }


    private void Update()
    {
        if(shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            if(shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                if (!endSmoothly)
                    noise.m_AmplitudeGain = 0/*Mathf.Lerp(startingIntensity, 0, shakeTimer / shakeTimerTotal)*/;
                else
                    noise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, shakeTimer / shakeTimerTotal);
            }

        }
    }
}