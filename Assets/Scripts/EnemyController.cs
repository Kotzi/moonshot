using UnityEngine;
using DG.Tweening;

public class EnemyController: MonoBehaviour
{
    protected const float Acceleration = 0.01f;

    protected bool IsActive = false;

    protected EarthController Earth;
    protected GameController GameController;
    protected float Velocity = 0f;

    public virtual void OnAwake() { }
    void Awake()
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
        GameController = GameObject.FindObjectOfType<GameController>();
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
        transform.localScale = Vector3.one * 0.15f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "Earth")
        {
            Earth.Destroyed();
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
        transform.DOScale(Vector3.one * 0.3f, 0.75f);
    }
}
