using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : AttackableController {

    override public void Start () {
        base.Start();
        this.tag = "Barrier";
    }

    override public void Update () {
        base.Update();
    }

    void OnCollisionEnter (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().velocity *= 1/2;
        }
    }

    void OnCollisionEnd (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().velocity *= 2;
        }
    }

    protected override void die () {

    }

}
