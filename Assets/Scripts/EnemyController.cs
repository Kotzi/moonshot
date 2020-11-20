using UnityEngine;

public class EnemyController: MonoBehaviour
{
    protected const float Acceleration = 0.01f;

    public bool IsActive = false;

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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "Earth")
        {
            print("you died!");
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
}
