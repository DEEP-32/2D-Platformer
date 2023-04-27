using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{

    private void Start()
    {
        ShakeCamera();
    }

    private void Update()
    {
        ShakeCamera();
    }

    public void ShakeCamera(float duration = .5f, float strength = 1f, Ease shakeEase = Ease.Linear)
    {
        //Debug.Log("Shaking the camera");
        transform.DOShakePosition(duration, strength).SetEase(shakeEase);
    }
}