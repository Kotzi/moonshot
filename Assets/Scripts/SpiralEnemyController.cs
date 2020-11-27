using UnityEngine;

public class SpiralEnemyController: EnemyController
{
    private float RotateSpeed = 0.75f;
    private float RadiusSpeed = 0.75f;
    private float Radius = 0f;
    private float Angle = 0f;
    private Vector3 Offset;
    private Camera Camera;

    public override void OnAwake()
    {
        Camera = Camera.main;
        RotateSpeed += Random.Range(0.25f, 1f);
        RadiusSpeed += Random.Range(0.25f, 1f);
        Offset = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
    }

    public override Vector3 Move(float time) 
    { 
        Angle += RotateSpeed * time;

        if(Radius < Earth.Radius) 
        {
            Radius += RadiusSpeed * time;
        }

        var oldPosition = transform.position;
        var newPosition = new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle), 0f) * Radius + Offset;
        Vector2 targetInViewportPosition = Camera.WorldToViewportPoint(newPosition);
        Vector3 clampedPosition = Camera.ViewportToWorldPoint(new Vector2(Mathf.Clamp(targetInViewportPosition.x, 0.05f, 0.95f), Mathf.Clamp(targetInViewportPosition.y, 0.05f, 0.95f)));
        clampedPosition.z = 10;
        transform.position = Vector3.Lerp(transform.position, clampedPosition, 0.5f);
        return (newPosition - oldPosition);
    }
}