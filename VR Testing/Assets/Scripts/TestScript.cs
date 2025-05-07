using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;
    public void OnLeftHandGrip(InputAction.CallbackContext context)
    {
        float gripValue = context.action.ReadValue<float>();
        leftHandAnimator.SetFloat("Grip", gripValue);
    }

    public void OnLeftHandTrigger(InputAction.CallbackContext context)
    {
        float triggerValue = context.action.ReadValue<float>();
        leftHandAnimator.SetFloat("Trigger", triggerValue);
    }

    public void OnRightHandGrip(InputAction.CallbackContext context)
    {
        float gripValue = context.action.ReadValue<float>();
        rightHandAnimator.SetFloat("Grip", gripValue);
    }

    public void OnRightHandTrigger(InputAction.CallbackContext context)
    {
        float triggerValue = context.action.ReadValue<float>();
        rightHandAnimator.SetFloat("Trigger", triggerValue);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        print(context.action.ReadValue<Vector2>());
    }
}
