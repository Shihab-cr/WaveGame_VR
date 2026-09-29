using UnityEngine;

public class FootStepsSFX_Handler : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioSource leftLegSource;
    [SerializeField] private AudioSource rightLegSource;
    public void PlayFootSteps()
    {
        if (footsteps.Length <= 0) return;
        if (leftLegSource != null)
        {
            int randIndex = Random.Range(0, footsteps.Length);
            AudioClip clip = footsteps[randIndex];
            leftLegSource.PlayOneShot(clip);
        }
        if(rightLegSource != null)
        {
            int randIndex = Random.Range(0, footsteps.Length);
            AudioClip clip = footsteps[randIndex];
            rightLegSource.PlayOneShot(clip);
        }
    }
}
