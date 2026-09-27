using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Accessibility;
public class PlayerInventory : MonoBehaviour
{
    private GameObject currWeapon;
    private Dictionary<AmmoEnum, int> playerAmmo = new Dictionary<AmmoEnum, int>();
    private MagazineHandler magHandler;
    private FireGun gun;
    private PlayerUIHandler playerUI;
    void Awake()
    {
        playerAmmo.Add(AmmoEnum.RifleAmmo, 200);
    }
    void Start()
    {
        playerUI = GetComponent<PlayerUIHandler>();
        if (playerUI != null)
        {
            playerUI.DrawInventoryUI(GetInventory());
        }
    }
    private void UpdateInventoryUI()
    {
        playerUI.UpdateInventoryUI(GetInventory());
    }
    public void AssignWeapon(GameObject weapon)
    {
        UnAssignWeapon();
        currWeapon = weapon;
        gun = currWeapon.GetComponent<FireGun>();
        magHandler = currWeapon.GetComponent<MagazineHandler>();
        gun.OnMagEmpty += ReloadGun;
    }
    public void UnAssignWeapon()
    {
        if (currWeapon == null) return;
        gun.OnMagEmpty -= ReloadGun;
        currWeapon = null;
    }
    public int GetAmmoFromInventory(AmmoEnum ammoType, int amount)
    {
        if (playerAmmo.ContainsKey(ammoType))
        {
            int value;
            playerAmmo.TryGetValue(ammoType, out value);
            if(value-amount >= 0)
            {
                value -= amount;
                playerAmmo[ammoType] = value;
                return amount;
            }
            else
            {
                playerAmmo[ammoType] = 0;
                return value;
            }
        }
        UpdateInventoryUI();
        return 0;
    }

    public GameObject GetCurrWeapon()
    {
        return currWeapon;
    }
    public void AddItemsToInventory(Dictionary<AmmoEnum, int> loot)
    {
        foreach(var item in loot)
        {
            if (playerAmmo.ContainsKey(item.Key))
            {
                playerAmmo[item.Key] += item.Value;
            }
            else
            {
                playerAmmo.Add(item.Key, item.Value);
            }
        }
        UpdateInventoryUI();
    }
    private void ReloadGun()
    {
        if(currWeapon != null)
        {
            
            if (gun == null) return;
            AmmoEnum gunAmmoType = gun.AmmoProperty;
            if (playerAmmo.ContainsKey(gunAmmoType))
            {
                playerAmmo[gunAmmoType] = magHandler.ReloadMag(playerAmmo[gunAmmoType]);
                Debug.Log("PlayerAmmo: " + playerAmmo[gunAmmoType]);
            }
        }
        UpdateInventoryUI();
    }
    public Dictionary<AmmoEnum, int> GetInventory()
    {
        return this.playerAmmo;
    }
    
}
