using UnityEngine;

public class MoonController: MonoBehaviour
{
    private float RotateSpeed = 1f;
    private float AttackSpeed = 4f;
    private float Radius;
    private float Angle;
    private float AddedDistance = 0f;

    void Start()
    {
        Radius = transform.localPosition.magnitude; // Only if Moon is always centered
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            Angle += RotateSpeed * Time.deltaTime;
        } 
        else if (Input.GetKey(KeyCode.DownArrow)) 
        {
            Angle -= RotateSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            AddedDistance += AttackSpeed * Time.deltaTime;
        }
        else 
        {
            AddedDistance = Mathf.Lerp(AddedDistance, 0f, Time.deltaTime*5f);
        }

        transform.localPosition = new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle), 0) * (Radius + AddedDistance);
    }
}
