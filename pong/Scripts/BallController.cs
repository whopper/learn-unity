using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    public float speed;
    public GameObject goal_1;
    public GameObject goal_2;

    private Rigidbody rbd;
    private Vector3 velocity;
    private Vector3 start_position;
    private GoalController gc_1;
    private GoalController gc_2;

  void Start () {
        rbd = GetComponent<Rigidbody> ();
        rbd.freezeRotation = true;
        rbd.drag = 0;
        rbd.angularDrag = 0;
        velocity = GetRandomVelocity ();
        rbd.velocity = velocity;
        start_position = transform.position;
        gc_1 = goal_1.GetComponent<GoalController> ();
        gc_2 = goal_2.GetComponent<GoalController> ();
  }

    public void reset () {
        transform.position = start_position;
        rbd.velocity = GetRandomVelocity ();
    }

    Vector3 GetRandomVelocity () {
        int x_val = Random.Range (3, 7);
        if (x_val > 8) {
            x_val = 5 - x_val;
        }

        int y_val = Random.Range (3, 7);
        if (y_val > 8) {
            y_val = 5 - y_val;
        }

        return new Vector3 (x_val, y_val, 0);
    }
}
