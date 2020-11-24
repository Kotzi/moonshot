using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{
    public GameObject GameOver;
    public GameObject YouWon; 
    private int CurrentWave = 0;
    private int CurrentWaveLength = 0;
    private EarthController Earth;
    private SunController Sun;

    void Awake() 
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
        Sun = GameObject.FindObjectOfType<SunController>();
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
                Sun.IncreaseIntensity();
                CurrentWaveLength = newWave.transform.childCount;
                foreach (var enemy in newWave.GetComponentsInChildren<EnemyController>())
                {
                    enemy.Activate();
                }
            }
            else 
            {
                YouWon.SetActive(true);
            }
        }
    }

    public void EnemyKilled()
    {
        CurrentWaveLength -= 1;
    }

    public void EarthDestroyed()
    {
        GameOver.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
