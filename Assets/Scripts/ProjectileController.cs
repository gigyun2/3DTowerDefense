﻿using System.Collections;
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
        base.Update();
        this.GetComponent<Rigidbody>().velocity = this.direction.normalized * this.flySpeed;
    }

    void OnCollisionEnter (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().Hurt(this.atk);
            die();
        }
    }

    protected override void die () {
        GameObject.Destroy(this);
    }
}
