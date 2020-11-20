using UnityEngine;

public class GameController: MonoBehaviour
{
    private int CurrentWave = 0;
    private int CurrentWaveLength = 0;
    private EarthController Earth;

    void Awake() 
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
    }

    void Update()
    {
        if(CurrentWaveLength == 0)
        {
            CurrentWave += 1;
            var newWave = GameObject.Find("EnemyGroup" + CurrentWave);
            if (newWave) 
            {
                Earth.IncreaseSpeed();
                CurrentWaveLength = newWave.transform.childCount;
                foreach (var enemy in newWave.GetComponentsInChildren<EnemyController>())
                {
                    enemy.IsActive = true;
                }
            }
            else 
            {
                print("YOU WON!");
            }
        }
    }

    public void EnemyKilled()
    {
        CurrentWaveLength -= 1;
    }
}
