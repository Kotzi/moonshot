using UnityEngine;

public class MoonController: MonoBehaviour
{
    private float AttackSpeed = 6f;
    private float MaxDistance = 3f;
    private float AddedDistance = 0f;
    
    void Update()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            AddedDistance = Mathf.Lerp(AddedDistance, MaxDistance, AttackSpeed * Time.deltaTime);
        }
        else 
        {
            AddedDistance = Mathf.Lerp(AddedDistance, 0f, Time.deltaTime*5f);
        }

        transform.localPosition = dir.normalized * AddedDistance;
    }
}
