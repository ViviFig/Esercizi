using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    public string TagName = "Terrain";
 	Transform t;
    Vector3 startPosition;
    Transform target;

    void Start() {
        t = transform;
        target = transform;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.tag != TagName)
                    return;
                target.position = hit.point;
            }

        }

    }
    
}
