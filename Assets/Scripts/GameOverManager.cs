using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;
    [SerializeField] HealthHandler player;
    void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }
    private void OnEnable()
    {
        if (player != null)
        {
            player.OnDeath += ActivateGameOverState;
        }
    }
    private void OnDisable()
    {
        if(player != null)
        {
            player.OnDeath -= ActivateGameOverState;
        }
    }
    private void ActivateGameOverState()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }
    public void DeActivateGameOverState()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }
}
