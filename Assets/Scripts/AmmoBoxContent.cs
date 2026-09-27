using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxContent : MonoBehaviour
{
    [SerializeField] private Dictionary<AmmoEnum, int> ammoBoxContent = new Dictionary<AmmoEnum, int>();
    private AmmoBoxLogic ammoBoxLogic;
    public Dictionary<AmmoEnum, int> GetBoxContent()
    {
        return ammoBoxContent;
    }
    private void Start()
    {
        ammoBoxLogic = GetComponentInParent<AmmoBoxLogic>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {

            if (ammoBoxLogic != null) {
                ammoBoxLogic.TransferContent(ammoBoxContent, col);
                Debug.Log("Player collected ammo");

            }
            else Debug.LogError("AmmoBoxLogic not found in parent");

        }
        
    }
}
