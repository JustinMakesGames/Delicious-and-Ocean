using System.Collections;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class Hand : MonoBehaviour
{
    #region Hands static Instances Setup

    public static Hand Left;
    public static Hand Right;

    private void Awake()
    {
        if (handType == HandType.Left)
        {
            Left = this;
        }
        else
        {
            Right = this;
        }
    }

    public HandType handType;
    public enum HandType
    {
        Left,
        Right,
    };

    #endregion

    public InteractionController interactionController;
}


