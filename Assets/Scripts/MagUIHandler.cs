using TMPro;
using UnityEngine;

public class MagUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI magTXT;
    public void UpdateMagUI(AmmoEnum ammoType,int bulletCount)
    {
        magTXT.text = ammoType + ":" + bulletCount;
    }
}
