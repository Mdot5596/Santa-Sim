using UnityEngine;

public class Present : MonoBehaviour
{
    [Tooltip("Numeric ID or type for this present (match with Sack.sackID)")]
    public int presentID = 0;

    [HideInInspector] public bool isBeingDragged = false;

    // Optional: called when present is accepted into sack
    public void OnAcceptedIntoSack(Sack sack)
    {
        // e.g. play sound, disable object, parent into sack, etc.
        // For now just destroy:
        Destroy(gameObject);
    }

    // Optional: called when rejected
    public void OnRejected()
    {
        // simple feedback or let it drop back to belt
    }
}
