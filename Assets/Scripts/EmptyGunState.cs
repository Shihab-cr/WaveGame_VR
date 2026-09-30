using UnityEngine;

public class EmptyGunState : IGunState
{
    private FireGun fireGun;
    private MagazineHandler magHandler;
    private float nextFire;
    private float reloadTime;
    public bool canFire = false;
    public EmptyGunState(FireGun fireGun)
    {
        this.fireGun = fireGun;
        if (fireGun != null)
        {
            this.nextFire = fireGun.NextFireCounter;
            this.reloadTime = fireGun.ReloadTime;
            magHandler = fireGun.MagazineHandlerProperty;
        }
    }
    public void PullTrigger()
    {
        if (fireGun == null) return;

        nextFire = fireGun.NextFireCounter;

        if (nextFire < Time.time)
        {
            nextFire = Time.time + reloadTime;
            fireGun.NextFireCounter = nextFire;
            // fireGun.CurrentGunStateProperty = fireGun.GunFireStateProperty;
            fireGun.Reload();
        }

    }
}