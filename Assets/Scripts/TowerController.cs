using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : AttackableController {
    public GameObject FirePoint;
    public GameObject Projectile; //Prefabs of projectile to shoot

    override public void Start () {
        base.Start();
        this.tag = "Tower";
    }

    override public void Update () {
        base.Update();
        if (cd <= 0) {
            foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
                if (Vector3.Distance(monster.transform.position, this.transform.position) < range) {
                    // face to target
                    Quaternion quaternion = Quaternion.LookRotation(monster.transform.position -
                        this.transform.GetChild(this.transform.childCount - 1).position);
                    quaternion.x = 0;
                    quaternion.z = 0;
                    this.transform.rotation = quaternion;

                    // shoot
                    if (Projectile != null) {
                        GameObject projectile = GameObject.Instantiate(Projectile);
                        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
                        projectileController.direction = (monster.transform.position - FirePoint.transform.position).normalized;
                        projectile.transform.position = FirePoint.transform.position + new Vector3(0, 1.57f, 0) + quaternion * Vector3.forward * 0.9f;// 2.9f);
                        projectileController.atk = this.atk;
                    }

                    cd = 1 / speed;
                    break;
                }
            }
        }
        else {
            cd -= Time.deltaTime;
        }
    }

    protected override void die () {

    }
}
