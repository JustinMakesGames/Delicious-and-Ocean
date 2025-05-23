using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : BaseEnemy
{
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    private bool _hasSpottedShip;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        HandleNormalBehaviour();
    }

    /*private void Update()
    {
        if (!_hasSpottedShip)
        {
            HandleNormalBehaviour();
        }
    }*/

    private void HandleNormalBehaviour()
    {
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            yield return StartCoroutine(JumpArc(transform.position, transform.position + transform.forward * jumpDistance, jumpHeight, jumpDuration));
        }
    }
    private IEnumerator JumpArc(Vector3 startPos, Vector3 endPos, float height, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            float t = time / duration;

            Vector3 midPoint = Vector3.Lerp(startPos, endPos, t);
            midPoint.y += height * 4 * (t - t * t);

            transform.position = midPoint;

            time += Time.deltaTime;
            yield return null;
        }


    }
}
