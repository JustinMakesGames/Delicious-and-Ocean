
using UnityEngine;

[Tooltip(" Enum to decide whether or not T can be hurt T = Type, which can be any type of class, struct or anything of the likes")]
public enum ImmunityState
{
    [Tooltip("T can be hurt")]
    Vulnerable,

    [Tooltip("T cannot be hurt")]
    Invulnerable,
}
