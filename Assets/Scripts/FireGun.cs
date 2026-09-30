using System;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
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
    private float reloadTime = 0.9f;
    private float nextFire = 0f;
    public float NextFireCounter { set => nextFire = value; get => nextFire; }
    public float FireRate { get => fireRate; }
    public float ReloadTime { get => reloadTime; }
    [SerializeField] private AmmoEnum requiredAmmoType;
    public AmmoEnum AmmoProperty { get => requiredAmmoType; }
    public event Action OnMagEmpty;
    private MagazineHandler magHandler;
    public MagazineHandler MagazineHandlerProperty { get => magHandler; }
    private MagUIHandler magUI;
    private AudioHandler audioHandler;

    private LoadedGunState gunFire;
    public LoadedGunState GunFireStateProperty { get => gunFire; }
    private EmptyGunState gunReload;
    public EmptyGunState GunReloadStateProperty { get => gunReload; }
    private Coroutine reloadCoroutine;
    private IGunState currState;
    public IGunState CurrentGunStateProperty { set => currState = value; get => currState; }
    void Start()
    {
        audioHandler = GetComponent<AudioHandler>();
        magHandler = GetComponent<MagazineHandler>();
        magUI = GetComponent<MagUIHandler>();
        if (magUI != null) magUI.UpdateMagUI(requiredAmmoType, magHandler.GetCurrBulletsCount());

       gunFire = new LoadedGunState(this);
       gunReload = new EmptyGunState(this);
       currState = gunFire;
    }
    public void Fire()
    {

        if (currState != null) currState.PullTrigger();
    }
    public void ShootBullets()
    {
        

        if (gunFireParticles != null) gunFireParticles.Emit(sparkBurstCount);

        Vector3 muzzleDirection = muzzleTip.forward;
        Debug.DrawRay(muzzleTip.position, muzzleDirection * 800f, Color.red, 0.2f);
        if (audioHandler != null) audioHandler.PlayRandomClipCategory1();
        if (Physics.Raycast(muzzleTip.position, muzzleDirection, out RaycastHit rayHit, Mathf.Infinity, hitLayers))
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
            EnemyVisual enemyVisual = rayHit.collider.GetComponent<EnemyVisual>();
            if (enemyVisual != null)
            {
                enemyVisual.PlayBloodVFX(rayHit.point, Quaternion.LookRotation(rayHit.normal));
            }

        }

        if (magUI != null)
        {
            magUI.UpdateMagUI(requiredAmmoType, magHandler.GetCurrBulletsCount());
        }
    }

    public void Reload()
    {
        if(reloadCoroutine == null)
        {
            reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
        
    }
    private IEnumerator ReloadCoroutine()
    {
        audioHandler.PlayRandomClipCategory3();
        yield return new WaitForSeconds(reloadTime);
        OnMagEmpty?.Invoke();
        if (magUI != null)
        {
            magUI.UpdateMagUI(requiredAmmoType, magHandler.GetCurrBulletsCount());
        }
        currState = gunFire;
        reloadCoroutine = null;
    }

}




