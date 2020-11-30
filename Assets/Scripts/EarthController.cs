using UnityEngine;

public class EarthController: MonoBehaviour
{
    private const float MaxShootingMoonTime = 0.05f;
    private const float ShootingMoonMaxCooldown = 0.25f;
    private const float RotatingMoonMaxCooldown = 1f;

    public float Radius = 0f;
    public GameObject Moon;
    private int Lifes = 10;
    private float RotateSpeed = 1f;
    private float Angle;
    private float AttackSpeed = 10f;
    private float MaxDistance = 3f;
    private float AddedDistance = 0f;
    private float ShootingMoonCooldown = 0f;
    private bool IsShootingMoon = false;
    private float ShootingMoonTime = 0f;
    private float RotatingMoonCooldown = 0f;
    private bool IsRotatingMoon = false;
    private float RotatingMoonStartingAngle = 0f;
    private float RotatingMoonAngle = 0f;
    private GameController GameController;

    void Awake()
    {
        GameController = Object.FindObjectOfType<GameController>();
        GameController.EarthLifesChanged(Lifes);
        var position = transform.position;
        position.z = 0;
        Radius = position.magnitude; // Only if Sun is always centered
    }

    void Update()
    {
        ShootingMoonCooldown -= Time.deltaTime;
        RotatingMoonCooldown -= Time.deltaTime;

        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Z)) && !IsShootingMoon && ShootingMoonCooldown <= 0f && AddedDistance < 0.1f)
        {
            IsShootingMoon = true;
        } else if ((Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.X)) && !IsRotatingMoon && RotatingMoonCooldown <= 0f)
        {
            IsRotatingMoon = true;

            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(Moon.transform.position);
            RotatingMoonStartingAngle = Mathf.Atan2(dir.y, dir.x);
            RotatingMoonAngle = RotatingMoonStartingAngle;
        }

        if (IsShootingMoon)
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
            ShootingMoonTime += Time.deltaTime;
            if (ShootingMoonTime >= MaxShootingMoonTime)
            {
                ShootingMoonTime = 0f;
                IsShootingMoon = false;
                ShootingMoonCooldown = ShootingMoonMaxCooldown;
            }
        }

        Angle += RotateSpeed * Time.deltaTime;

        transform.position = new Vector3(1.5f*Mathf.Sin(Angle), Mathf.Cos(Angle), 10f) * Radius;

        if (IsRotatingMoon) 
        {
            RotatingMoonAngle += 30 * RotateSpeed * Time.deltaTime;
            if (RotatingMoonAngle <= (2*Mathf.PI + RotatingMoonStartingAngle)) 
            {
                Moon.transform.rotation = Quaternion.AngleAxis(RotatingMoonAngle * Mathf.Rad2Deg, Vector3.forward);
            } 
            else 
            {
                IsRotatingMoon = false;
                RotatingMoonCooldown = RotatingMoonMaxCooldown;
            }
        } 
        else 
        {
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(Moon.transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Moon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Moon.transform.localPosition = dir.normalized * AddedDistance;
        }

        GameController.UpdateMoonShotSlider(1 - (ShootingMoonCooldown/ShootingMoonMaxCooldown));
        GameController.UpdateMoonShieldSlider(1 - (RotatingMoonCooldown/RotatingMoonMaxCooldown));
    }

    public void Attacked(EnemyController enemy)
    {
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        var enemySr = enemy.GetComponent<SpriteRenderer>();
        sr.sprite = enemy.SmallSprite;
        sr.sortingOrder = enemySr.sortingOrder;
        sr.sortingLayerName = enemySr.sortingLayerName;
        sr.sortingLayerID = enemySr.sortingLayerID;
        go.transform.parent = this.transform;
        Vector3 position = Random.insideUnitCircle;
        position.z = 10;
        go.transform.localPosition = position;

        Lifes -= 1;
        GameController.EarthLifesChanged(Lifes);
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
