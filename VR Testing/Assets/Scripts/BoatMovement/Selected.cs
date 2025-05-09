using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Selected : XRGrabInteractable
{
    private Vector3 _originalPosition;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        _originalPosition = transform.localPosition;
    }

    protected override void ProcessInteractionStrength(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractionStrength(updatePhase);

        if (isSelected)
        {
            transform.localPosition = _originalPosition;
        }
    }
}
