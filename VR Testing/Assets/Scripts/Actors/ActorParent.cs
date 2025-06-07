using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorParent : ActorBase
{
    private List<ActorChild> _actorChildren = new List<ActorChild>();

    #region Instantiation
    protected GameObject InstantiateChild(GameObject childGO, Transform childTrans)
    {

        var child = Instantiate(childGO, childTrans.position, childTrans.rotation, transform);

        if (child.TryGetComponent(out ActorChild AC))
        {
            AC.ParentActor = this;
            _actorChildren.Add(AC);
        }
        else
        {
            Debug.LogWarning("ActorChild component not found on the instantiated child GameObject: " + childGO.name);
        }

        return child;
    }
    protected GameObject InstantiateChild(GameObject childGO, Transform childTrans, Transform parentTransform)
    {
        var child = Instantiate(childGO, childTrans.position, childTrans.rotation, parentTransform);

        if (child.TryGetComponent(out ActorChild AC))
        {
            AC.ParentActor = this;
            _actorChildren.Add(AC);
        }
        else
        {
            Debug.LogWarning("ActorChild component not found on the instantiated child GameObject: " + childGO.name);
        }

        return child;
    }
    protected GameObject InstantiateChild(GameObject childGO, Vector3 spawnPos)
    {
        var child = Instantiate(childGO, spawnPos, Quaternion.identity, transform);

        if (child.TryGetComponent(out ActorChild AC))
        {
            AC.ParentActor = this;
            _actorChildren.Add(AC);
        }
        else
        {
            Debug.LogWarning("ActorChild component not found on the instantiated child GameObject: " + childGO.name);
        }

        return child;
    }
    protected GameObject InstantiateChild(GameObject childGO, Vector3 spawnPos, Transform parentTransform)
    {
        var child = Instantiate(childGO, spawnPos, Quaternion.identity, parentTransform);

        if (child.TryGetComponent(out ActorChild AC))
        {
            AC.ParentActor = this;
            _actorChildren.Add(AC);
        }
        else
        {
            Debug.LogWarning("ActorChild component not found on the instantiated child GameObject: " + childGO.name);
        }

        return child;
    }
    #endregion

    protected override void OnActorDeath()
    {
        for(int i = 0; i < _actorChildren.Count; i++)
        {
            if (_actorChildren[i] != null)
            {
                //Kill all the children
                _actorChildren[i].OnDamageTaken(int.MaxValue, DamageType.Physical);
            }
        }
        base.OnActorDeath();
    }
}
