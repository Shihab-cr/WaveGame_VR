using TMPro;
using UnityEngine;

public class SpawnerUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyCountUI;
    public void SetEnemyCountText(int currCount, int maxCount)
    {
        if (enemyCountUI != null) enemyCountUI.text = currCount.ToString() + "/" + maxCount.ToString();
    }
}
