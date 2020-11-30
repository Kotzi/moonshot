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
        if(CurrentWaveLength == 0 && !YouWon.activeSelf && !GameOver.activeSelf)
        {
            CurrentWave += 1;
            WavesText.text = "Wave " + CurrentWave;
            var originalScale = WavesText.transform.localScale;
            WavesText.transform.DOScale(originalScale * 1.3f, 0.7f).OnComplete(() => {
                WavesText.transform.DOScale(originalScale, 0.3f);
            });

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