using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    private void Start()
    {
        AudioManagement.Instance.PlayAudio("Explosion");
        Destroy(gameObject, 1f);
    }
}
