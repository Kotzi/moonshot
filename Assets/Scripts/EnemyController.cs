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

    public virtual void OnAwake() { }
    void Awake()
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
        GameController = GameObject.FindObjectOfType<GameController>();
        Animator = GetComponent<Animator>();
        OnAwake();
    }

    void Update()
    {
        if(IsActive && Earth)
        {
            Velocity += Acceleration;
            var position = Earth.transform.position;

            Vector3 targetDirection = position - transform.position;
            if(targetDirection != Vector3.zero)
            {
                transform.position += targetDirection * Velocity * Time.deltaTime;
            }
            else 
            {
                transform.position = position;
            }
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
