using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheelHandler : XRBaseInteractable
{
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;
    [SerializeField] private float revertSpeed;

    [SerializeField] private bool shouldRotateBack;
    [SerializeField] private float steerMaximum;

    public UnityEvent<float> OnWheelRotated;

    private float currentAngle = 0.0f;


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        currentAngle = FindWheelAngle();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        currentAngle = FindWheelAngle();
    }

    
    private void Update()
    {
        if (!isSelected && shouldRotateBack)
        {
            RevertTheSteeringWheel();
        }
        
        
    }
    
    //Reverts the steering wheel to its normal state when not interacting with it anymore
    private void RevertTheSteeringWheel()
    {
        float zRotation = NormalizeAngle(wheelTransform.eulerAngles.z);

        if (zRotation < minRotation)
        {
            wheelTransform.Rotate(transform.forward, revertSpeed, Space.World);
        }

        else if (zRotation > maxRotation)
        {
            wheelTransform.Rotate(transform.forward, -revertSpeed, Space.World);
        }
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    //Handles the steer rotation
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                RotateWheel();
        }
    }

    private void RotateWheel()
    {
        // Convert that direction to an angle, then rotation
        float totalAngle = FindWheelAngle();

        // Apply difference in angle to wheel
        float angleDifference = currentAngle - totalAngle;

        currentAngle += angleDifference;

       
        wheelTransform.Rotate(transform.forward, -angleDifference, Space.World);


        // Store angle for next process
        currentAngle = totalAngle;
        OnWheelRotated?.Invoke(angleDifference);






    }

    private float FindWheelAngle()
    {
        float totalAngle = 0;

        // Combine directions of current interactors
        foreach (IXRSelectInteractor interactor in interactorsSelecting)
        {
            Vector2 direction = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
        }

        return totalAngle;
    }

    private Vector2 FindLocalPoint(Vector3 position)
    {
        // Convert the hand positions to local, so we can find the angle easier
        return transform.InverseTransformPoint(position).normalized;
    }

    private float ConvertToAngle(Vector2 direction)
    {
        // Use a consistent up direction to find the angle
        return Vector2.SignedAngle(Vector2.up, direction);
    }

    private float FindRotationSensitivity()
    {
        // Use a smaller rotation sensitivity with two hands
        return 1.0f / interactorsSelecting.Count;
    }
}