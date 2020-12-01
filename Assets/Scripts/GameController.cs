using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;
using Cinemachine;
using System.Collections;

public class GameController: MonoBehaviour
{
    private const float MaxShakingTime = 0.25f;
    public Text WavesText;
    public Text LifesText;
    public Slider MoonShotSlider;
    public Slider MoonShieldSlider;
    public GameObject StartUI;
    public GameObject GameUI;
    public GameObject GameOver;
    public Text GameOverWavesText;
    public GameObject YouWon;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public Light2D BackgroundLight;
    public CinemachineVirtualCamera MainVirtualCamera;
    private int CurrentWave = 0;
    private EarthController Earth;
    private SunController Sun;
    private CinemachineBasicMultiChannelPerlin Noise;
    private AudioClip HitSound;
    private bool IsShaking = false;
    private float ShakingTime = 0f;
    private bool ShouldUpdate = false;

    void Awake() 
    {
        HitSound = Resources.Load<AudioClip>("lava");
        Earth = GameObject.FindObjectOfType<EarthController>();
        Sun = GameObject.FindObjectOfType<SunController>();
        Noise = MainVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

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
        if (ShouldUpdate)
        {
            var oldWave = GameObject.Find("EnemyGroup" + CurrentWave);

            if (!oldWave || oldWave.transform.childCount == 0)
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
                    BackgroundLight.intensity = Mathf.Clamp(BackgroundLight.intensity + 0.05f, 0f, 2f);
                    foreach (var enemy in newWave.GetComponentsInChildren<EnemyController>())
                    {
                        enemy.Activate();
                    }
                }
                else 
                {
                    YouWon.SetActive(true);
                    ShouldUpdate = false;
                }
            }
        }

        if (IsShaking) 
        {
            ShakingTime -= Time.deltaTime;
            if (ShakingTime <= 0) 
            {
                UpdateNoise(0f, 0f);
                IsShaking = false;
            }
        }
    }

    public void EarthLifesChanged(int lifes)
    {
        LifesText.text = "Lifes: " + lifes;
        var originalScale = LifesText.transform.localScale;
        LifesText.transform.DOScale(originalScale * 0.8f, 0.7f).OnComplete(() => {
            LifesText.transform.DOScale(originalScale, 0.3f);
        });
        ShakeCamera();
    }

    public void UpdateMoonShotSlider(float value)
    {
        MoonShotSlider.value = value;
    }

    public void UpdateMoonShieldSlider(float value)
    {
        MoonShieldSlider.value = value;
    }

    public void EnemyDestroyed(bool attacked)
    {
        if (attacked)
        {
            AudioSource.PlayClipAtPoint(HitSound, transform.position);
        }
    }

    public void EarthDestroyed()
    {
        ShouldUpdate = false;
        GameOverWavesText.text = "But hey, you survived " + CurrentWave + " waves!";
        GameOver.SetActive(true);
    }

    public void RestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartClicked()
    {
        StartUI.SetActive(false);
        GameUI.SetActive(true);
        MainVirtualCamera.Priority = 10000;
        StartCoroutine(DelayedStartGame());
    }

    IEnumerator DelayedStartGame()
    {
        yield return new WaitForSeconds(2);
 
        ShouldUpdate = true;
    }

    private void ShakeCamera() 
    {
        UpdateNoise(2f, 2f);
        IsShaking = true;
        ShakingTime = MaxShakingTime;
    }

    private void UpdateNoise(float amplitudeGain, float frequencyGain) 
    {
        if (Noise) 
        {
            Noise.m_AmplitudeGain = amplitudeGain;
            Noise.m_FrequencyGain = frequencyGain;
        }
    }
}