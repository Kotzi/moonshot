using UnityEngine;

public class PreflyEnemyController: EnemyController
{
    private Vector3 PreflyPosition = Vector3.zero;
    private bool PreflyCompleted = false;
    private float MaxTimeSincePreflyCompleted = 1f;
    private float TimeSincePreflyCompleted = 0f;

    public override void OnAwake()
    {
        MaxTimeSincePreflyCompleted += Random.Range(-0.5f, 1f);
    }

    public override void Activate()
    {
        var xModifier = Random.Range(0, 2) == 0 ? -1 : 1;
        var yModifier = Random.Range(0, 2) == 0 ? -1 : 1;
        var position = new Vector3(Earth.transform.position.x * xModifier + Random.Range(-2f, 2f), Earth.transform.position.y * yModifier + Random.Range(-2f, 2f), 10);
        var camera = Camera.main;
        Vector2 targetInViewportPosition = camera.WorldToViewportPoint(position);
        Vector3 clampedPosition = camera.ViewportToWorldPoint(new Vector2(Mathf.Clamp(targetInViewportPosition.x, 0.05f, 0.95f), Mathf.Clamp(targetInViewportPosition.y, 0.05f, 0.95f)));
        clampedPosition.z = 10;
        PreflyPosition = clampedPosition;

        base.Activate();
    }

    public override Vector3 Move(float time) 
    { 
        if (PreflyCompleted) 
        {
            if (TimeSincePreflyCompleted > MaxTimeSincePreflyCompleted)
            {
                Vector3 targetDirection = Earth.transform.position - transform.position;
                var movement = targetDirection * Velocity * time;
                transform.position += movement;
                return movement;
            }
            else 
            {
                TimeSincePreflyCompleted += time;
                return Earth.transform.position - transform.position; // Fake movement to follow the Earth
            }
        }
        else
        {
            Vector3 targetDirection = PreflyPosition - transform.position;

            if(Vector3.SqrMagnitude(targetDirection) > 0.05)
            {
                var movement = targetDirection * Velocity * time;
                transform.position += movement;
                return movement;
            }
            else 
            {
                transform.position = PreflyPosition;
                PreflyCompleted = true;
                Animator.SetBool("HasFace", true);
                return Earth.transform.position - transform.position; // Fake movement to follow the Earth
            }
        }
    }
}