using UnityEngine;
using System.Collections;

public class BaseController : MonoBehaviour {

    public GameObject infantry;
    public GameObject tank;

    public void spawn_unit (string unit_name) {
        Random.InitState (Random.Range(1, 100));

        GameObject obj;
        // TODO: This is dumb. Just abstract this out and instantate whatever we've given
        if (unit_name == "Tank") {
            obj = (GameObject)Instantiate (tank, transform.position, Quaternion.identity);
        } else {
            obj = (GameObject)Instantiate (infantry, transform.position, Quaternion.identity);
        }

        obj.transform.Translate (new Vector3 (Random.Range(0.2f, 0.8f), Random.Range(-0.1f, -0.3f), 0));
        obj.transform.Rotate (new Vector3 (0, 180, 0));
    }
}
