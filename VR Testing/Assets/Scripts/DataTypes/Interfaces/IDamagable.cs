using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    [Tooltip("This function is called when T takes damage")]
    public void OnDamageTaken(int damage);
}
// signed by Canpai