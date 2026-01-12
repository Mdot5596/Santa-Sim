using UnityEngine;

public class Sack : MonoBehaviour
{
    public int sackID = 0;

    [Header("Feedback")]
    public Light feedbackLight;
    public float lightFlashDuration = 0.2f;
public ParticleSystem successParticles;

    public bool TryAcceptPresent(Present p)
    {
        if (p == null) return false;

        if (p.presentID == sackID)
        {
            GameManager.Instance.AddScore(p.presentID);
            p.OnAcceptedIntoSack(this);

            TriggerFeedback();
            return true;
        }

        return false;
    }

    void TriggerFeedback()
    {
        if (feedbackLight != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashLight());
        }

        if (successParticles != null)
        {
            successParticles.Play();
        }
    }


    System.Collections.IEnumerator FlashLight()
    {
        feedbackLight.intensity = 5f;
        yield return new WaitForSeconds(lightFlashDuration);
        feedbackLight.intensity = 0f;
    }
}
