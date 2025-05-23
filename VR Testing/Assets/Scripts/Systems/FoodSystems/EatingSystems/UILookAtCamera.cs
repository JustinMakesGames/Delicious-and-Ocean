using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{

    private void Start()
    {
        Destroy(gameObject, 1);
    }

    private void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}
