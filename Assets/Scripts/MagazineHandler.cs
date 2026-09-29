using UnityEngine;

public class MagazineHandler : MonoBehaviour
{
    [SerializeField] private int magSize = 30;
    private AudioHandler audioHandler;
    private void Start()
    {
        audioHandler = GetComponent<AudioHandler>();
    }
    private int currBulletNum = 30;
    
    
    public void UseBullet()
    {
        currBulletNum--;
        
    }
    public int ReloadMag(int amount)
    {
        if (audioHandler != null) audioHandler.PlayRandomClipCategory2();
        int missingBullets = magSize - currBulletNum;
        if (amount >= missingBullets)
        {
            currBulletNum = magSize;
            return amount - missingBullets;
        }
        else
        {
            currBulletNum += amount;
            return 0;
        }
    }
    public bool IsMagEmpty()
    {   
        return currBulletNum <= 0;
    }
    public int GetCurrBulletsCount()
    {
        return this.currBulletNum;
    }
    
}
