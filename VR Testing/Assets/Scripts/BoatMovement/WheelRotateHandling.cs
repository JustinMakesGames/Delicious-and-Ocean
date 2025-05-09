using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelRotateHandling : XRSimpleInteractable
{
    [SerializeField] private Transform handToLookAt;

    public void Selected(SelectEnterEventArgs args)
    {
        print("Has selected");
        handToLookAt = args.interactorObject.transform;
    }

    private void Update()
    {
        if (isSelected)
        {
            
        }
    }
}
