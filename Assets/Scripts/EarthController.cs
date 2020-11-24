using UnityEngine;

public class EarthController: MonoBehaviour
{
    private const float MaxSpeed = 2.5f;
    private const float MaxAttackTime = 0.05f;
    public float Radius = 0f;
    public GameObject Moon;
    private int Lifes = 3;
    private float RotateSpeed = 0.3f;
    private float Angle;
    private float AttackSpeed = 10f;
    private float MaxDistance = 3f;
    private float AddedDistance = 0f;
    private bool IsAttacking = false;
    private float AttackTime = 0f;
    private GameController GameController;

    void Awake()
    {
        GameController = Object.FindObjectOfType<GameController>();
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

        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) && !IsAttacking && AddedDistance < 0.1f)
        {
            IsAttacking = true;
        }

        if (IsAttacking)
        {
            AddedDistance = Mathf.Lerp(AddedDistance, MaxDistance, AttackSpeed * Time.deltaTime);
        }
        else if (AddedDistance > 0f)
        {
            AddedDistance = Mathf.Lerp(AddedDistance, -1f, Time.deltaTime * AttackSpeed * 2f);
        } 
        else 
        {
            AddedDistance = Mathf.Lerp(AddedDistance, 0f, Time.deltaTime * AttackSpeed * 3f);
        }

        if ((MaxDistance - AddedDistance) < 0.01)
        {
            AttackTime += Time.deltaTime;
            if (AttackTime >= MaxAttackTime)
            {
                AttackTime = 0f;
                IsAttacking = false;
            }
        }

        Moon.transform.localPosition = dir.normalized * AddedDistance;
    }

    public void IncreaseSpeed()
    {
        RotateSpeed = Mathf.Clamp(RotateSpeed * 1.25f, 0f, MaxSpeed);
    }

    public void Attacked()
    {
        Lifes -= 1;
        if (Lifes <= 0)
        {
            Destroyed();
        }
    }
    void Destroyed()
    {
        // Do something nice
        Destroy(gameObject);
        GameController.EarthDestroyed();
    }
}
