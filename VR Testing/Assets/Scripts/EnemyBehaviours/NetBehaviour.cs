using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private Transform positionToDrop;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float returnSpeed;
    private bool _canThrow;
    private bool _hasBeenThrown;

    public void CanThrow(ActivateEventArgs args)
    {     
        _canThrow = !_canThrow;
        print(_canThrow);
    }
    public void ThrowNet(SelectExitEventArgs args)
    {
        if (!_canThrow) return;
        _hasBeenThrown = true;
        StartCoroutine(ScaleUp());

    }

    private IEnumerator ScaleUp()
    {
        while (transform.localScale.magnitude < maxScale.magnitude)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, maxScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }


    }

    private void GatherFish(Transform pool)
    {
        List<Transform> fishList = new List<Transform>();
        
        for (int i = 0; i < pool.childCount; i++)
        {
            float distance = Vector3.Distance(transform.position, pool.GetChild(i).position);

            if (distance < maxScale.x)
            {
                fishList.Add(pool.GetChild(i));
            }
        }

        foreach (Transform fish in fishList)
        {
            fish.parent = transform;
            fish.GetComponent<FishEnemy>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pool") && _hasBeenThrown)
        {
            GatherFish(other.transform);
            StartCoroutine(MoveNetTowardsPlayer());
            
        }
    }

    private IEnumerator MoveNetTowardsPlayer()
    {
        
        yield return new WaitForSeconds(1);

        while (Vector3.Distance(transform.position, positionToDrop.position) < 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToDrop.position, returnSpeed * Time.deltaTime);
        }

        List<Transform> fishList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            fishList.Add(transform.GetChild(i));
            fishList[i].parent = null;
        }
    }
}
