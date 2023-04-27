using UnityEngine;
using DG.Tweening;
public class PlayerAnimationController : MonoBehaviour
{
    PlayerController2 playerController;

    private void Update()
    {
        if (playerController.IsIdle)
        {
            HandleIdleAnimation();
        }
    }



    private void HandleIdleAnimation()
    {
    }

}
