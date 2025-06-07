using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("A child actor that sends input damage to its parent, usefull for multi-piece bosses/enemies")]
public class ActorChild : ActorBase
{
    protected ActorBase _parentActor;
    public ActorBase ParentActor
    {
        get => _parentActor;
        set
        {
            if (_parentActor == null)
            {
                _parentActor = value;
            }

        }
    }

    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        _parentActor?.OnDamageTaken(damage, dmgType);

        base.OnDamageTaken(damage, dmgType);
    }
}
