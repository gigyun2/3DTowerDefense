using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

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
        if (route.Count > 0 && isAlive) {
            Vector3 direction = (route[0] - this.transform.position);
            if (Mathf.Abs(direction.x) + Mathf.Abs(direction.z) < 0.1f) {
                route.RemoveAt(0);
            }
            //direction.y = 0;
            // Lerp
            direction = direction.normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
                this.transform.rotation = Quaternion.LookRotation(direction);
            this.GetComponent<Rigidbody>().velocity = direction * velocity;
        } else {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }

    void LateUpdate () {

    }

    void OnCollisionStay (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null) {
            if (this.cd <= 0) {
                // attack
                if (collidedObject.tag.Equals("Barrier")) {
                    collidedObject.GetComponent<BarrierController>().Hurt(this.atk);
                }
                if (collidedObject.tag.Equals("Character")) {
                    collidedObject.GetComponent<FirstPersonController>().Hurt(this.atk);
                }
                this.cd = 1 / this.speed;
            }
        }
    }

    protected override void die () {
        FirstPersonController playerController
            = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        playerController.money += 100;
        //Destroy(this.gameObject);
        this.GetComponent<Collider>().enabled = false;
        StartCoroutine(Dying(1.5f));
    }

    IEnumerator Dying (float time)
    {
        float t = 0;
        float c = 0;
        while (t < time) {
            c = Mathf.Lerp(1.0f, 0.4f, t);
            transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color
                = new Color(c, c, c);
            t += Time.deltaTime * (1.0f / time);
            if (t >= time) {
                Destroy(this.gameObject);
            }
            yield return null;
        }
        Destroy(this.gameObject);
        yield return null;
    }
}
