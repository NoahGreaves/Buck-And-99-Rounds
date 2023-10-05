using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// referenced from: https://github.com/SebLague/Field-of-View/blob/master/Episode%2001/Scripts/FieldOfView.cs
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _viewRadius;
    [Range(0f, 360f)]
    [SerializeField] private float _viewAngle = 80;

    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask; // Set walls and other obstacles and OBSTACLE layer
    
    private readonly List<Transform> _visbleTargets = new List<Transform>();

    public float GetViewRadius { get { return _viewRadius; } }
    public float GetViewAngle { get { return _viewAngle; } }
    public LayerMask GetTargetMask { get { return _targetMask; } }

    public List<Transform> GetVisibleTargets{ get { return _visbleTargets; } }

    private void Update()
    {
        FindVisibleTargets();
    }

    private void FindVisibleTargets() 
    {
        _visbleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);
            if (angleToTarget < _viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, _obstacleMask)) 
                {
                    _visbleTargets.Add(target);
                }
            }
        }
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) 
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
