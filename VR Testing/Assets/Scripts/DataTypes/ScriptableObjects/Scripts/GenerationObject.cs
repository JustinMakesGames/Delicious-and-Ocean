using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GenerationObject")]
public class GenerationObject : ScriptableObject
{
    [Tooltip("The prefab of the object, which will be instantiated")]
    public GameObject objectGO;

    [Tooltip("The position of the object to be instantiated relative to the position of the chunk")]
    public Vector3 setRelativeObjectPos;

    [Tooltip("The additive position of the objects relative to its set spawningposition, only used if relatieveObjectPos is equal to vector3.zero")]
    public Vector3 relativeObjectAdditivePos;

    [Tooltip("Whether or not multiple instances of this may be generated")]
    public bool multipleInstancesAllowed = true;

}
