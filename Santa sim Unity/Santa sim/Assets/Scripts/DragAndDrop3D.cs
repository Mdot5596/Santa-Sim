using UnityEngine;

public class DragAndDrop3D : MonoBehaviour
{
    Camera cam;
    Present grabbed;
    Vector3 grabOffset;          // offset between hit point and object pivot
    Plane dragPlane;             // plane used for projection
    float planeHeight = 1.0f;    // set to a height in your scene suitable for dragging

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMouse();
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickup();
        }
        else if (Input.GetMouseButton(0) && grabbed != null)
        {
            ContinueDrag();
        }
        else if (Input.GetMouseButtonUp(0) && grabbed != null)
        {
            Release();
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Present p = hit.collider.GetComponentInParent<Present>();
            if (p != null)
            {
                grabbed = p;
                grabbed.isBeingDragged = true;
                // stop conveyor movement while dragging:
                ConveyorMover mover = grabbed.GetComponent<ConveyorMover>();
                if (mover != null) mover.move = false;

                // create drag plane at the object's current vertical position (or fixed height)
                planeHeight = grabbed.transform.position.y;
                dragPlane = new Plane(Vector3.up, new Vector3(0, planeHeight, 0));

                // compute offset so the object doesn't snap pivot to mouse position
                Vector3 hitPoint = hit.point;
                grabOffset = grabbed.transform.position - hitPoint;

                // make rigidbody kinematic so collisions don't interfere while dragging
                Rigidbody rb = grabbed.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }
    }

    void ContinueDrag()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPos = ray.GetPoint(enter);
            grabbed.transform.position = hitPos + grabOffset;
        }
    }

    void Release()
    {
        // On release, check nearby sacks (or rely on sack trigger)
        Collider[] hits = Physics.OverlapSphere(grabbed.transform.position, 0.6f);
        Sack found = null;
        foreach (var col in hits)
        {
            Sack s = col.GetComponentInParent<Sack>();
            if (s != null)
            {
                found = s;
                break;
            }
        }

        if (found != null)
        {
            // let sack handle acceptance
            if (found.TryAcceptPresent(grabbed))
            {
                // accepted - sack can destroy or parent the present
            }
            else
            {
                // rejected - simple feedback and drop back onto belt
                grabbed.OnRejected();
                ReturnToBelt(grabbed);
            }
        }
        else
        {
            // No sack found: drop back to belt / resume movement
            ReturnToBelt(grabbed);
        }

        grabbed.isBeingDragged = false;
        // restore physics/movement
        Rigidbody rb = grabbed.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // keep kinematic while moving by script
        ConveyorMover mover = grabbed.GetComponent<ConveyorMover>();
        if (mover != null) mover.move = true;

        grabbed = null;
    }

    void ReturnToBelt(Present p)
    {
        // Option A: let the conveyor continue moving and the present will slide.
        // Option B: teleport back to belt spawn or nearest belt line.
        // Here we do nothing special; the mover resumes and continues moving.
    }
}
