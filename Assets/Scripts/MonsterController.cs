using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : AttackableController {
    public List<Vector3> route;
    public float velocity;
    public int reward;

    override public void Start () {
        base.Start();
        this.tag = "Monster";
    }

    override public void Update () {
        base.Update();
        if (route.Count > 0) {
            Vector3 direction = (route[0] - this.transform.position);
            if (Mathf.Abs(direction.x) + Mathf.Abs(direction.z) < 0.02) {
                route.RemoveAt(0);
                Debug.Log(route.Count);
            }
            direction.y = 0;
            // Lerp
            direction = direction.normalized;
            if (direction != Vector3.zero)
                this.transform.rotation = Quaternion.LookRotation(direction);
            this.GetComponent<Rigidbody>().velocity = direction * velocity;
        } else {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }

    void LateUpdate () {
        if (route.Count > 0) {
            Vector3 temp = this.transform.position;
            temp.y = 0;
            if (Vector3.Distance(temp, route[0]) < 0.1f) {
                route.RemoveAt(0);
            }
        }
    }

    void OnCollisionStay (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null) {
            if (this.cd <= 0) {
                // attack
                if (collidedObject.tag.Equals("Barrier")) {
                    collidedObject.GetComponent<BarrierController>().Hurt(this.atk);
                }
                this.cd = 1 / this.speed;
            }
        }
    }

    protected override void die () {
        // give reawrd to palyer
    }
}
