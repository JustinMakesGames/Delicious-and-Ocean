using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotationFixedWeapon : MonoBehaviour
{
    public void FixRotation(SelectExitEventArgs args)
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = args.interactorObject.transform.GetComponent<TrackHandPosition>().Velocity;
        Vector3 direction = rb.velocity.normalized;
        if (direction != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(direction);
        }
    }
}
