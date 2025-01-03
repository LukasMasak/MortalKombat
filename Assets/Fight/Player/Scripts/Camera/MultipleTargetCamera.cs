using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;

    public Vector3 offset;

    private Vector3 velocity;
    public float smoothTime = .5f;

    public float maxZoom = 50f;
    public float minZoom = 10f;
    public float zoomLimiter = 50f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestdistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestdistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (targets == null || targets.Count == 0) return Vector3.zero;

        if (targets.Count == 1) return targets[0].position;
        
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
