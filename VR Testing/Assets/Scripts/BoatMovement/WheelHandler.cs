using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelHandler : MonoBehaviour
{
    [SerializeField] private Transform handle;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float drag;
    private Vector3 originalPosition;
    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        originalPosition = transform.localPosition;
        print("Grabbed this");
        Transform hand = args.interactorObject.transform.GetChild(0);

        hand.SetParent(handle);
        hand.position = handle.position;
        rb.drag = drag;
        rb.angularDrag = drag;
    }

    public void Processing(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        transform.localPosition = originalPosition;
    }
    public void OnSelectExit(SelectExitEventArgs args)
    {
        Transform hand = handle.GetChild(0);

        hand.SetParent(args.interactorObject.transform);
        hand.SetSiblingIndex(0);

        rb.drag = Mathf.Infinity;
        rb.angularDrag = Mathf.Infinity;
    }
}
