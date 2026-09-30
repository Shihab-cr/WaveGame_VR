using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int waveNum = 0;
    [SerializeField] private int breakDuration = 10;
    [SerializeField] private Spawner spawner;
    private bool isBreak = false;
    private float timer = 0f;
    private WaveUIHandler waveUI;
    void Start()
    {
        waveUI = GetComponent<WaveUIHandler>();
        if(waveUI != null)
        {
            waveUI.DisableWaveBreakTimer();
            waveUI.EnableWaveNumTxt();
        }
        SetUpNextWave();
    }
    void Update()
    {
        if (isBreak)
        {
            HandleBreakTimer();
        }
    }
    void StartWaveBreakTimer()
    {
        isBreak = true;
    }
    void HandleBreakTimer()
    {
        if (timer < breakDuration)
        {
            timer += Time.deltaTime;
            if(waveUI != null)
            {
                waveUI.DisableWaveNumTxt();
                waveUI.EnableWaveBreakTimer();
                waveUI.UpdateWaveBreakTimer((int)timer);
            }
        }
        else
        {
            if (waveUI != null)
            {
                waveUI.DisableWaveBreakTimer();
                waveUI.EnableWaveNumTxt();
            }
            timer = 0f;
            isBreak = false;
            SetUpNextWave();
        }
    }
    void SetUpNextWave()
    {
        waveNum++;
        if (waveUI != null)
        {
            waveUI.EnableWaveNumTxt();
            waveUI.UpdateWaveNumTxt(waveNum);
        }
        int waveCount = waveNum * 5;
        spawner.SetSpawnCount(waveCount);
        spawner.StartSpawning();
    }
    void OnEnable()
    {
        spawner.OnWaveCompleted += StartWaveBreakTimer;
    }
    void OnDisable()
    {
        spawner.OnWaveCompleted -= StartWaveBreakTimer;
    }

    public void ResetWave()
    {
        this.waveNum = 0;
        StartWaveBreakTimer();
    }
}
