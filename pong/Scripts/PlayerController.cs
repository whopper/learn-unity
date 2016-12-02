using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Text count_text;
    public GameObject goal;
    public GameObject ball;
    public string vertical_input_key;

    private Rigidbody rbd;
    private int count;
    private GoalController gc;
    private BallController bc;

    void Start () {
        rbd = GetComponent<Rigidbody> ();
        rbd.freezeRotation = true;
        rbd.drag = 1;

        gc = goal.GetComponent<GoalController> ();
        bc = ball.GetComponent<BallController> ();

        count = 0;
        count_text.text = count.ToString ();
    }

    void FixedUpdate () {
        float move_v = Input.GetAxis (vertical_input_key);
        rbd.velocity = new Vector3 (0, move_v * speed, 0);
        if (gc.scored) {
          bc.reset ();
          count++;
          count_text.text = count.ToString ();
          gc.scored = false;
        }
    }
}
