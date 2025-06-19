using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BarrelHandler : MonoBehaviour
{
    public int amountOfSpears;
    [SerializeField] private TMP_Text amountOfSpearText;
    [SerializeField] private XRInteractionManager interactionManager;
    [SerializeField] private GameObject weapon;
    [SerializeField] private bool isInfinite;
    [SerializeField] private Transform ship;
    [SerializeField] private int maxNumberChance;
    [SerializeField] private GameObject bombSpear;

    private void Awake()
    {
        amountOfSpearText.text = amountOfSpears.ToString();

        if (isInfinite) 
        {
            amountOfSpearText.text = "∞";
        }
    }
    public void InstantiateSpear(SelectEnterEventArgs args)
    {
        if (amountOfSpears <= 0) return;

        int randomBombSpearChance = Random.Range(0, maxNumberChance);

        GameObject woodenSpearClone = null;
        if (randomBombSpearChance == 0)
        {
            woodenSpearClone = Instantiate(bombSpear, transform.position, Quaternion.identity, ship);
        }

        else
        {
            woodenSpearClone = Instantiate(weapon, transform.position, Quaternion.identity, ship);
        }

        XRGrabInteractable xrGrab = woodenSpearClone.GetComponent<XRGrabInteractable>();

        interactionManager.SelectEnter(args.interactorObject, xrGrab);

        if (!isInfinite ) 
        {
            amountOfSpears--;
            amountOfSpearText.text = amountOfSpears.ToString();
        }
        
    }

    public void GetSpear()
    {
        amountOfSpears++;
        amountOfSpearText.text = amountOfSpears.ToString();
    }
}
