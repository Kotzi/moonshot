using UnityEngine;

public class GameController: MonoBehaviour
{
    private int CurrentWave = 0;
    private int CurrentWaveLength = 0;

    void Update()
    {
        if(CurrentWaveLength == 0)
        {
            CurrentWave += 1;
            var newWave = GameObject.Find("EnemyGroup" + CurrentWave);
            if (newWave) 
            {
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
