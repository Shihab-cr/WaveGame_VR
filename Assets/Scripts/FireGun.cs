using System;
using UnityEngine;

public class FireGun : MonoBehaviour
{
    [SerializeField] private Transform muzzleTip;
    [SerializeField] private float gunDmg = 35;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private ParticleSystem gunFireParticles;
    [SerializeField] private ParticleSystem collisionParticles;
    [SerializeField] private int sparkBurstCount = 15;
    [SerializeField] private float fireRate = 0.1f;
    private float nextFire = 0f;
    [SerializeField] private AmmoEnum requiredAmmoType;
    public AmmoEnum AmmoProperty { get => requiredAmmoType; }
    public event Action OnMagEmpty;
    private MagazineHandler magHandler;
    private MagUIHandler magUI;
    void Start()
    {
        magHandler = GetComponent<MagazineHandler>();
        magUI = GetComponent<MagUIHandler>();
        if (magUI != null) magUI.UpdateMagUI(requiredAmmoType, magHandler.GetCurrBulletsCount());
    }
    public void Fire()
    {
        if (magHandler != null && magHandler.IsMagEmpty())
        {
            Debug.Log("Mag is empty");
            OnMagEmpty?.Invoke();
            return;
        }
        
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
        }
        else
        {
            return;
        }

        if (magHandler != null) magHandler.UseBullet();

        if (gunFireParticles != null)
        {
            gunFireParticles.Emit(sparkBurstCount);
            
        }
        Vector3 muzzleDirection = muzzleTip.forward;
        Debug.DrawRay(muzzleTip.position, muzzleDirection*800f, Color.red, 0.2f);
        if(Physics.Raycast(muzzleTip.position, muzzleDirection, out RaycastHit rayHit, Mathf.Infinity, hitLayers))
        {
            HealthHandler enemyHealth = rayHit.collider.GetComponent<HealthHandler>();
            Debug.Log("Ray hit zombie");
            if (enemyHealth != null) enemyHealth.DecrementHealth(gunDmg);
            else
            {
                if (collisionParticles == null) return;
                collisionParticles.transform.position = rayHit.point;
                collisionParticles.transform.rotation = Quaternion.LookRotation(rayHit.normal);
                collisionParticles.Play();
            }
            
        }

        if(magUI != null)
        {
            magUI.UpdateMagUI(requiredAmmoType, magHandler.GetCurrBulletsCount());
        }
    }
}
