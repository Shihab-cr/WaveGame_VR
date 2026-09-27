using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthTXT;
    [SerializeField] private TextMeshProUGUI inventoryItemUI;
    [SerializeField] private Transform invUIContainer;
    private HealthHandler playerHealth;
    private List<TextMeshProUGUI> inventoryTXT=new List<TextMeshProUGUI>();
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
    public void DrawInventoryUI(Dictionary<AmmoEnum, int> inv)
    {
        int index = 0;
        foreach(var item in inv)
        {
            TextMeshProUGUI inventoryItem = Instantiate(inventoryItemUI);
            inventoryItem.transform.SetParent(invUIContainer);
            inventoryItem.text = item.Key + " : " + item.Value;
            inventoryItem.transform.position = new Vector3(127, -187f-68*index, 0f);
            inventoryTXT.Add(inventoryItem);
            index++;
        }
    }
    public void UpdateInventoryUI(Dictionary<AmmoEnum,int> inv)
    {
        int index = 0;
        foreach(var item in inv)
        {
            if (!inventoryTXT[index].text.Equals(item.Key + ":" + item.Value))
            {
                TextMeshProUGUI itemTxt = inventoryTXT[index];
                itemTxt.text = item.Key + ":" + item.Value;
            }
        }
    }
}
