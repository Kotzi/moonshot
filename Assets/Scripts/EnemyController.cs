using UnityEngine;
using DG.Tweening;

public class EnemyController: MonoBehaviour
{
    protected const float Acceleration = 0.01f;

    protected bool IsActive = false;
    protected EarthController Earth;
    protected GameController GameController;
    protected float Velocity = 0f;
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;

    public virtual void OnAwake() { }
    void Awake()
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
        GameController = GameObject.FindObjectOfType<GameController>();
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        OnAwake();
    }

    public virtual Vector3 Move(float time) 
    { 
        var targetDirection = Earth.transform.position - transform.position;

        if(targetDirection != Vector3.zero)
        {
            var increment = targetDirection * Velocity * Time.deltaTime;
            transform.position += increment;
            return increment;
        }
        else 
        {
            transform.position = Earth.transform.position;
            return Vector3.zero;
        }
    }

    void Update()
    {
        if(IsActive && Earth)
        {
            Velocity += Acceleration;
            var movement = Move(Time.deltaTime);
            bool flipSprite = (SpriteRenderer.flipX ? (movement.x > 0f) : (movement.x < 0f));
            if (flipSprite) 
            {
                SpriteRenderer.flipX = !SpriteRenderer.flipX;
            }

            // var direction = transform.rotation * Vector2.right;
            // var targetDirection = Earth.transform.position - transform.position;
            // var angleDiff = Vector2.SignedAngle(direction, targetDirection);
            // var clampedDiff = Mathf.Clamp(
            //     angleDiff,
            //     -100 * Time.deltaTime,
            //     100 * Time.deltaTime
            // );

            // transform.rotation = Quaternion.AngleAxis(
            //     transform.eulerAngles.z + clampedDiff,
            //     Vector3.forward
            // );
        }
    }

    void Start()
    {
        transform.localScale = Vector3.one * 2f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "Earth")
        {
            Earth.Attacked();
            Destroyed();
        }
        else if (collider.name == "Moon")
        {
            Destroyed();
        }
    }

    void Destroyed()
    {
        GameController.EnemyKilled();
        Destroy(gameObject);
    }

    public void Activate() 
    {
        IsActive = true;
        Animator.enabled = true;
        transform.DOScale(Vector3.one * 2.5f, 0.75f);
    }
}
