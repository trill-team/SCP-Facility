using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallbehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll) {
        print("it's okay");
        if (coll.gameObject.tag == "wall") {
            Destroy(this.gameObject);
        }
    }
}
