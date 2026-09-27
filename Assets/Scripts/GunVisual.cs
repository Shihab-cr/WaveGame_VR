using UnityEngine;

public class GunVisual : MonoBehaviour
{
    private GameObject weapon;
    [SerializeField] private Transform weaponOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon == null) return;
        weapon.transform.position = weaponOffset.position;
        weapon.transform.rotation = weaponOffset.rotation;
    }
    public void SetGun(GameObject gun)
    {
        this.weapon = gun;
    }
}
