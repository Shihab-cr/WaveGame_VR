using UnityEngine;

public class LoadedGunState : IGunState
{
    private FireGun fireGun;
    private MagazineHandler magHandler;
    private float nextFire;
    private float fireRate;
    public LoadedGunState(FireGun fireGun)
    {
        this.fireGun = fireGun;
        if (fireGun != null)
        {
            this.nextFire = fireGun.NextFireCounter;
            this.fireRate = fireGun.FireRate;
            magHandler = fireGun.MagazineHandlerProperty;

        }
    }
    public void PullTrigger()
    {
        if (fireGun == null) return;

        nextFire = fireGun.NextFireCounter;

        if (magHandler.IsMagEmpty())
        {
            fireGun.CurrentGunStateProperty = fireGun.GunReloadStateProperty;
            fireGun.CurrentGunStateProperty.PullTrigger();
        }
        else if (nextFire < Time.time)
        {
            nextFire = Time.time + fireRate;
            fireGun.NextFireCounter = nextFire;
            magHandler.UseBullet();
            fireGun.ShootBullets();
        }

    }
}
