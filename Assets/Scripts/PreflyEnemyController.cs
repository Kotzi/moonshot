using UnityEngine;

public class PreflyEnemyController: EnemyController
{
    private Vector3 PreflyDirection = Vector3.zero;
    private bool PreflyCompleted = false;

    public override void OnAwake()
    {
        var xModifier = Random.Range(0, 2) == 0 ? -1 : 1;
        var yModifier = Random.Range(0, 2) == 0 ? -1 : 1;
        var position = new Vector3(Earth.transform.position.x * xModifier + Random.Range(-2, 2), Earth.transform.position.y * yModifier + Random.Range(-2, 2), 10);
        var camera = Camera.main;
        Vector2 targetInViewportPosition = camera.WorldToViewportPoint(position);
        Vector3 clampedPosition = camera.ViewportToWorldPoint(new Vector2(Mathf.Clamp(targetInViewportPosition.x, 0.05f, 0.95f), Mathf.Clamp(targetInViewportPosition.y, 0.05f, 0.95f)));
        clampedPosition.z = 10;
        PreflyDirection = clampedPosition;
    }

    public override Vector3 Move(float time) 
    { 
        var position = PreflyCompleted ? Earth.transform.position : PreflyDirection;

        Vector3 targetDirection = position - transform.position;

        if(Vector3.SqrMagnitude(targetDirection) > 0.01)
        {
            var movement = targetDirection * Velocity * time;
            transform.position += movement;
            return movement;
        }
        else 
        {
            transform.position = position;
            PreflyCompleted = true;
            return Vector3.zero;
        }
    }
}