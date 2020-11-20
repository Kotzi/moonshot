using UnityEngine;

public class SpiralEnemyController: EnemyController
{
    private float RotateSpeed = 1f;
    private float RadiusSpeed = 1f;
    private float Radius = 0f;
    private float Angle = 0f;
    private Vector3 Offset;

    public override void OnAwake()
    {
        RotateSpeed += Random.Range(0.25f, 2f);
        RadiusSpeed += Random.Range(0.5f, 2f);
        Offset = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
    }

    void Update()
    {
        if(IsActive && Earth)
        {
            Angle += RotateSpeed * Time.deltaTime;

            if(Radius < Earth.Radius) 
            {
                Radius += RadiusSpeed * Time.deltaTime;
            }
    
            transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle), 0f) * Radius + Offset, 0.5f);
        }
    }
}