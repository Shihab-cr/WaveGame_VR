using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthTXT;
    private HealthHandler playerHealth;
    [SerializeField]private Dictionary<AmmoEnum,TextMeshProUGUI> inventoryTXT=new Dictionary<AmmoEnum,TextMeshProUGUI>();
    private void Start()
    {
        playerHealth = GetComponent<HealthHandler>();
        if (playerHealth != null)
        {
            playerHealth.OnHit += UpdateHealthTxt;
            playerHealth.OnDeath += UpdateHealthTxt;
        }
    }
    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHit -= UpdateHealthTxt;
            playerHealth.OnDeath -= UpdateHealthTxt;
        }
    }
    public void UpdateHealthTxt()
    {
        float health = playerHealth.getHealth();
        if(healthTXT != null) healthTXT.text = "Health: " + ((int)(health+0.5f)).ToString();
    }
    public void DisplayInventoryUI(Dictionary<AmmoEnum, int> inv)
    {
        foreach(var item in inv)
        {
            foreach(var display in inventoryTXT)
            {
                if (item.Key.Equals(display.Key))
                {
                    display.Value.text = item.Key + " : " + item.Value;
                    break;
                }
            }
        }
    }
}
