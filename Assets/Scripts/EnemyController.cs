using UnityEngine;

public class EnemyController: MonoBehaviour
{
    private const float Acceleration = 0.05f;

    private EarthController Earth;
    private float Velocity = 0f;

    void Awake()
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
    }

    void Update()
    {
        if(Earth)
        {
            Velocity += Acceleration;

            Vector3 targetDirection = Earth.transform.position - transform.position;
            if(targetDirection.magnitude >= 1.5f)
            {
                transform.position += targetDirection * Velocity * Time.deltaTime;
            }
            else 
            {
                transform.position = Earth.transform.position;
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
        Destroy(gameObject);
    }
}
