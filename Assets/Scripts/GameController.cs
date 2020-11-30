using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameController: MonoBehaviour
{
    public Text WavesText;
    public Text LifesText;
    public Slider MoonShotSlider;
    public Slider MoonShieldSlider;
    public GameObject GameOver;
    public Text GameOverWavesText;
    public GameObject YouWon;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    private int CurrentWave = 0;
    private int CurrentWaveLength = 0;
    private EarthController Earth;
    private SunController Sun;

    void Awake() 
    {
        Earth = GameObject.FindObjectOfType<EarthController>();
        Sun = GameObject.FindObjectOfType<SunController>();

        for(int i = 1; i <= 100; i++)
        {
            var group = new GameObject();
            group.name = "EnemyGroup" + i;
            group.transform.parent = transform.parent;
            var enemyCount = Random.Range(1, 5);
            for(int j = 0; j < enemyCount; j++)
            {
                var position = (Vector3)Random.insideUnitCircle;
                position.z = 10;
                GameObject enemy = null;
                switch (Random.Range(0, 3))
                {
                    case 0:
                        enemy = Enemy1;
                        break;
                    case 1:
                        enemy = Enemy2;
                        break;
                    case 2:
                        enemy = Enemy3;
                        break;
                }
                Instantiate(enemy, position, Quaternion.identity, group.transform);
            }
        }
    }

    void Update()
    {
        if(CurrentWaveLength == 0 && !YouWon.activeSelf && !GameOver.activeSelf)
        {
            CurrentWave += 1;
            WavesText.text = "Wave: " + CurrentWave;
            var originalScale = WavesText.transform.localScale;
            WavesText.transform.DOScale(originalScale * 1.3f, 0.7f).OnComplete(() => {
                WavesText.transform.DOScale(originalScale, 0.3f);
            });

            var newWave = GameObject.Find("EnemyGroup" + CurrentWave);
            if (newWave) 
            {
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

    public void EarthLifesChanged(int lifes)
    {
        LifesText.text = "Lifes: " + lifes;
    }

    public void UpdateMoonShotSlider(float value)
    {
        MoonShotSlider.value = value;
    }

    public void UpdateMoonShieldSlider(float value)
    {
        MoonShieldSlider.value = value;
    }

    public void EarthDestroyed()
    {
        GameOverWavesText.text = "You survived " + (CurrentWave) + " waves";
        GameOver.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}