using UnityEngine;

public class EarthController: MonoBehaviour
{
    private const float MaxSpeed = 3f;
    public float Radius = 0f;
    public GameObject Moon;
    private float RotateSpeed = 0.3f;
    private float Angle;
    private float AttackSpeed = 6f;
    private float MaxDistance = 3f;
    private float AddedDistance = 0f;

    void Awake()
    {
        var position = transform.position;
        position.z = 0;
        Radius = position.magnitude; // Only if Sun is always centered
    }

    void Update()
    {
        Angle += RotateSpeed * Time.deltaTime;
 
        transform.position = new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle), 10f) * Radius;

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(Moon.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Moon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            AddedDistance = Mathf.Lerp(AddedDistance, MaxDistance, AttackSpeed * Time.deltaTime);
        }
        else 
        {
            AddedDistance = Mathf.Lerp(AddedDistance, 0f, Time.deltaTime*5f);
        }

        Moon.transform.localPosition = dir.normalized * AddedDistance;
    }

    public void IncreaseSpeed()
    {
        RotateSpeed = Mathf.Clamp(RotateSpeed * 1.25f, 0f, MaxSpeed);
    }
}
