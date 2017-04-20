using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : AttackableController {
    public GameObject Projectile; //Prefabs of projectile to shoot
    private Transform FirePoint;

    override public void Start () {
        base.Start();
        this.tag = "Tower";
        FirePoint = this.transform.GetChild(this.transform.childCount - 1).FindChild("FirePoint");

        // every level increase size 25%, atk 50%, speed 0.2, range 0.5
        int level = 1;
		if (PlayerPrefs.HasKey ("Tower1")) {
			PlayerPrefs.GetInt ("Tower1");
		}
        // TODO: change scalelevel to 0.25 after demo
		this.transform.localScale = new Vector3 (0.2f, 0.15f, 0.2f) * (1 + level * 0.5f);
		this.atk = (int)(10 * (1 + 0.5 * (level - 1)));
		this.speed = (float)(0.4 + 0.2 * (level - 1));
		this.range = (float)(3 + 0.5 * (level - 1));
    }

    override public void Update () {
        base.Update();
        if (cd <= 0) {
            foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
                if (monster.GetComponent<MonsterController>().isAlive &&
                    Vector3.Distance(monster.transform.position, this.transform.position) < range) {
                    // face to target
                    Vector3 direction = monster.transform.position -
                        this.transform.GetChild(this.transform.childCount - 1).position;
                    Quaternion quaternion = Quaternion.LookRotation(direction);
                    //quaternion.x = 0;
                    //quaternion.z = 0;
                    //this.transform.rotation = quaternion;
                    quaternion.x *= 0.3f;
                    quaternion.z *= 0.3f;
                    this.transform.GetChild(1).rotation = quaternion;

                    // shoot
                    if (Projectile != null) {
                        GameObject projectile = GameObject.Instantiate(Projectile);
                        ProjectileController projectileController =
                            projectile.GetComponent<ProjectileController>();
                        Vector3 face = new Vector3(direction.x, 0, direction.z);
                        projectile.transform.position = FirePoint.transform.position;// +
                            //new Vector3(0, 1.518f, 0) + face * 0.3f;
                        projectileController.direction =
                            (monster.transform.position - projectile.transform.position +
                            monster.GetComponent<Rigidbody>().velocity * 0.2f).normalized;
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
