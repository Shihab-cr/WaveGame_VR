using TMPro;
using UnityEngine;

public class WaveUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveNumTxt;
    [SerializeField] private TextMeshProUGUI waveBreakTimer;
    
    public void DisableWaveBreakTimer()
    {
        if (waveBreakTimer != null) waveBreakTimer.gameObject.SetActive(false);
    }
    public void DisableWaveNumTxt()
    {
        if (waveNumTxt != null) waveNumTxt.gameObject.SetActive(false);
    }
    public void EnableWaveBreakTimer()
    {
        if (waveBreakTimer != null) waveBreakTimer.gameObject.SetActive(true);
    }
    public void EnableWaveNumTxt()
    {
        if (waveNumTxt != null) waveNumTxt.gameObject.SetActive(true);
    }
    public void UpdateWaveNumTxt(int waveNum)
    {
        if(waveNumTxt != null)
        {
            waveNumTxt.text = "Wave: " + waveNum.ToString();
        }
    }
    public void UpdateWaveBreakTimer(int duration)
    {
        int seconds = duration % 60;
        int minutes = duration / 60;
        if(waveBreakTimer !=null) waveBreakTimer.text = $"{minutes:00}:{seconds:00}";
    }
}
