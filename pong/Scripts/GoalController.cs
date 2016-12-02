using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

    public bool scored;

    void Start () {
        scored = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Ball") {
            scored = true;
        }
    }
}
