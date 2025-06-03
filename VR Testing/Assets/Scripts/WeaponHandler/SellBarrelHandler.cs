using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SellBarrelHandler : MonoBehaviour
{
    [SerializeField] private XRInteractionManager interactionManager;
    [SerializeField] private GameObject woodenSpear;
    public void InstantiateSpear(SelectEnterEventArgs args)
    {
        GameObject woodenSpearClone = Instantiate(woodenSpear, transform.position, Quaternion.identity);

        XRGrabInteractable xrGrab = woodenSpearClone.GetComponent<XRGrabInteractable>();

        interactionManager.SelectEnter(args.interactorObject, xrGrab);
    }
}
