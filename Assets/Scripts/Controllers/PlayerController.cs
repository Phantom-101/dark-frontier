using System;
using UnityEngine;

[Serializable]
public class PlayerController : Controller {

    public override void Control (Structure structure) {

        if (Input.GetMouseButtonDown (0)) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hit)) {

                GameObject obj = hit.collider.transform.parent.gameObject;
                Structure str = obj.GetComponent<Structure> ();
                if (str != structure) structure.SetTarget (str);

            }

        }

    }

}
