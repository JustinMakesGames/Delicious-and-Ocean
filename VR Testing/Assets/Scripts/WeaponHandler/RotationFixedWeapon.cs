using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotationFixedWeapon : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    private bool _hasBeenThrown;
    private Transform _boss;

    private bool _isInBossDirection;
    public void FixRotation(SelectExitEventArgs args)
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = args.interactorObject.transform.GetComponent<TrackHandPosition>().Velocity;
        Vector3 direction = rb.velocity.normalized;
        if (direction != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(direction);
        }

        _hasBeenThrown = true;

        if (GameObject.FindGameObjectWithTag("Boss") != null)
        {
            _boss = GameObject.FindGameObjectWithTag("Boss").transform;
        }
    }

    private void Update()
    {
        if (_hasBeenThrown && _boss)
        {
            CheckIfCloseToBoss();
        }
    }

    private void CheckIfCloseToBoss()
    {
        if (Vector3.Distance(transform.position, _boss.position) < maxDistance)
        {
            RotateToBoss();
        }

        if (_isInBossDirection)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void RotateToBoss()
    {
        transform.rotation = ReturnRotation();


    }

    private Quaternion ReturnRotation()
    {
        Vector3 direction = (_boss.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        return lookRotation;
    }
}
