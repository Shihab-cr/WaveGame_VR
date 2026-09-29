using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Separate audio collections")]
    [SerializeField] private AudioClip[] clipsCategory1;
    [SerializeField] private AudioClip[] clipsCategory2;
    [SerializeField] private AudioClip[] clipsCategory3;
    [SerializeField] private AudioClip[] clipsCategory4;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }
    public void PlayRandomClipCategory1()
    {
        if (clipsCategory1.Length > 0)
        {
            int index = Random.Range(0, clipsCategory1.Length);
            AudioClip clipToPlay = clipsCategory1[index];
            if (audioSource != null) AudioSource.PlayClipAtPoint(clipToPlay, transform.position);
        }
    }
    public void PlayRandomClipCategory2()
    {
        if (clipsCategory2.Length > 0)
        {
            int index = Random.Range(0, clipsCategory2.Length);
            AudioClip clipToPlay = clipsCategory2[index];
            if (audioSource != null) AudioSource.PlayClipAtPoint(clipToPlay, transform.position);
        }
    }
    public void PlayRandomClipCategory3()
    {
        if (clipsCategory3.Length > 0)
        {
            int index = Random.Range(0, clipsCategory3.Length);
            AudioClip clipToPlay = clipsCategory3[index];
            if (audioSource != null) AudioSource.PlayClipAtPoint(clipToPlay, transform.position);
        }
    }
    public void PlayRandomClipCategory4()
    {
        if (clipsCategory4.Length > 0)
        {
            int index = Random.Range(0, clipsCategory4.Length);
            AudioClip clipToPlay = clipsCategory4[index];
            if (audioSource != null) audioSource.PlayOneShot(clipToPlay);
        }
    }
}
