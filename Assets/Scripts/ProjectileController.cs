using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : AttackableController {
    public float flySpeed;
    public Vector3 direction;

    override public void Start () {
        base.Start();
        this.tag = "Projectile";
    }

    override public void Update () {
        this.GetComponent<Rigidbody>().velocity = direction.normalized * flySpeed;
    }

    void OnTriggerEnter (Collider collider) {
        GameObject collidedObject = collider.gameObject.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().Hurt(atk);
        }
        die();
    }

    protected override void die () {
        GameObject.Destroy(this.gameObject);
    }
}
