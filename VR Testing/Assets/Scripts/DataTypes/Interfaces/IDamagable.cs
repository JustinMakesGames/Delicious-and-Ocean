using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum DamageType
{
    None = 0,   
    Physical = 1,
    Fire = 2 ,
    Ice = 4,
    Poison = 8,
    Electric = 16,
    Blunt = 32,
    Sharp = 64,
    Oil = 128,
    All = Physical | Fire | Ice | Poison | Electric | Blunt | Sharp | Oil
}
public interface IDamagable
{
    [Tooltip("This function is called when T takes damage")]
    public void OnDamageTaken(int damage, DamageType dmgType = DamageType.Physical);
}
// signed by Canpai