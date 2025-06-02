using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRHandAnimator : MonoBehaviour
{
    private Animator anim;

    private float controllerButtonPressPercent;
    private float _cButtonPressPercent;
    public float valueUpdateSpeed;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnBigTriggerStateChange(InputAction.CallbackContext ctx)
    {
        controllerButtonPressPercent = ctx.ReadValue<float>();
    }

    private void Update()
    {
        if (_cButtonPressPercent == controllerButtonPressPercent)
        {
            return;
        }

        _cButtonPressPercent = Mathf.MoveTowards(_cButtonPressPercent, controllerButtonPressPercent, valueUpdateSpeed * Time.deltaTime);
        anim.SetFloat("GrabStrength", _cButtonPressPercent);
    }
}
