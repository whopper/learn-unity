using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime; // Time it takes object to move in seconds
    public LayerMask blockingLayer; // Which layer to detect collisions in.

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    private float inverseMoveTime; // Make movement calculations more efficient.

    protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D> ();
        rb2d = GetComponent <Rigidbody2D> ();
        inverseMoveTime = 1f / moveTime; // Multiplying is more efficient than dividing
    }

    // out means arg is passed by reference. We're using this func to return the bool and alter hit.
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position; // Implicit conversion from v3 to v2, discards z axis data
        Vector2 end = start + new Vector2 (xDir, yDir);

        boxCollider.enabled = false; // so we don't hit our own collider
        hit = Physics2D.Linecast (start, end, blockingLayer); // Cast a line from start point to end point checking collisions on blocking layer
        boxCollider.enabled = true;

        if (hit.transform == null) {
            // space was open
            StartCoroutine(SmoothMovement (end));
            return true;
        }
        return false; // Move unsuccessful
    }

    // Move units from one square to the next
    protected IEnumerator SmoothMovement (Vector3 end) {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude; // Distance between where we are and destination. sqrMagnitude is cheaper than Magnitude.
        while (sqrRemainingDistance > float.Epsilon) {  // float.Ep is a very small number... almost 0.
            // This returns a point that's somewhat closer to our destination based on move time.
            Vector3 newPosition = Vector3.MoveTowards (rb2d.position, end, inverseMoveTime + Time.deltaTime);
            rb2d.MovePosition (newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude; // Recalculate remaining distance.
            yield return null; // Wait for a frame before reevaluating the condition of the loop.
        }
    }

    // Use template so we can run into either enemies or walls
    protected virtual void AttemptMove <T> (int xDir, int yDir) 
        where T : Component 
    {
        RaycastHit2D hit;
        bool canMove = Move (xDir, yDir, out hit);
        if (hit.transform == null) {
            return;
        }

        T hitComponent = hit.transform.GetComponent <T> ();
        if (!canMove && hitComponent != null) {
            OnCantMove (hitComponent);
        }
    }

    protected abstract void OnCantMove <T> (T component) // implemented in children classes
        where T: Component;
}
