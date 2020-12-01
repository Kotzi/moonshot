using UnityEngine;

public class SpiralEnemyController: EnemyController
{
    private float MaxFlyTime = 10f; 
    private float RotateSpeed = 0.75f;
    private float RadiusSpeed = 0.75f;
    private float Radius = 0f;
    private float Angle = 0f;
    private float TotalFlyTime = 0f;
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
        if (TotalFlyTime < MaxFlyTime) 
        {
            TotalFlyTime += time;

            var factor = 0.25f;
            if (TotalFlyTime > 1f)
            {
                factor = 0.5f;
                Angle += RotateSpeed * time;

                if(Radius < Earth.Radius) 
                {
                    Radius += RadiusSpeed * time;
                }
                else 
                {
                    Radius -= RadiusSpeed * time * Random.Range(1f, 1.5f);
                }
            }

            var oldPosition = transform.position;
            var newPosition = new Vector3(1.5f*Mathf.Sin(Angle), Mathf.Cos(Angle), 0f) * Radius + Offset;
            Vector2 targetInViewportPosition = Camera.WorldToViewportPoint(newPosition);
            Vector3 clampedPosition = Camera.ViewportToWorldPoint(new Vector2(Mathf.Clamp(targetInViewportPosition.x, 0.05f, 0.95f), Mathf.Clamp(targetInViewportPosition.y, 0.05f, 0.95f)));
            clampedPosition.z = 10;
            transform.position = Vector3.Lerp(transform.position, clampedPosition, factor);
            return (newPosition - oldPosition);
        }
        else
        {
            var targetDirection = Earth.transform.position - transform.position;

            if(targetDirection != Vector3.zero)
            {
                var increment = targetDirection * Velocity * 0.5f * Time.deltaTime;
                transform.position += increment;
                return increment;
            }
            else 
            {
                transform.position = Earth.transform.position;
                return Vector3.zero;
            }
        }
    }
}