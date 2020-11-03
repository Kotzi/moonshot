using UnityEngine;

public class EarthController: MonoBehaviour
{
    private float RotateSpeed = 0.5f;
    private float Radius;
    private float Angle;

    void Start()
    {
        Radius = transform.position.magnitude; // Only if Sun is always centered
    }

    void Update()
    {
        Angle += RotateSpeed * Time.deltaTime;
 
        transform.position = new Vector2(Mathf.Sin(Angle), Mathf.Cos(Angle)) * Radius;
    }
}
