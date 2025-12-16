using UnityEngine;

public class Sack : MonoBehaviour
{
    [Tooltip("Which present ID this sack accepts")]
    public int sackID = 0;

    [Tooltip("Optional snap point where accepted presents get parented")]
    public Transform acceptedParent;

    private void Reset() {
        // ensure collider exists & is trigger for easy OnTriggerEnter
        Collider col = GetComponent<Collider>();
        if (col == null) gameObject.AddComponent<BoxCollider>().isTrigger = true;
        else col.isTrigger = true;
    }

    // Used by DragAndDrop on release
    public bool TryAcceptPresent(Present p)
    {
        if (p == null) return false;

        if (p.presentID == sackID)
        {
            if (acceptedParent != null)
            {
                p.transform.position = acceptedParent.position;
                p.transform.SetParent(acceptedParent, true);
            }
            else
            {
                p.transform.SetParent(transform, true);
            }

            GameManager.Instance.AddScore(p.presentID);           
            p.OnAcceptedIntoSack(this);
            return true;
        }
        else
        {
            // rejected
            return false;
        }
    }

private void OnTriggerEnter(Collider other)
{
    Present p = other.GetComponent<Present>();

    if (p != null && p.isBeingDragged && p.presentID == sackID)
    {
        p.OnAcceptedIntoSack(this);
    }
}
}