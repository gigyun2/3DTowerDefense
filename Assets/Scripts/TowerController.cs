using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : AttackableController {
    public GameObject FirePoint;
    public GameObject Projectile; //Prefabs of projectile to shoot

    override public void Start () {
        base.Start();
        this.tag = "Tower";

		// every level increase size 25%, atk 50%, speed 0.2, range 0.5
		int level = 1;
		if (PlayerPrefs.HasKey ("Tower1")) {
			PlayerPrefs.GetInt ("Tower1");
		}
		this.transform.localScale = new Vector3 (0.25, 0.25, 0.25) * (1 + level * 0.25);
		this.atk = (int)(10 * (1 + 0.5 * (level - 1)));
		this.speed = (float)(0.4 + 0.2 * (level - 1));
		this.range = (float)(3 + 0.5 * (level - 1));
    }

    override public void Update () {
        base.Update();
        if (cd <= 0) {
            foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
                if (Vector3.Distance(monster.transform.position, this.transform.position) < this.range) {
                    // face to target
                    Quaternion quaternion = Quaternion.LookRotation(monster.transform.position -
                        this.transform.GetChild(this.transform.childCount - 1).position);
                    quaternion.x = 0;
                    quaternion.z = 0;
                    this.transform.rotation = quaternion;

                    // shoot
                    if (Projectile != null) {
                        GameObject projectile = GameObject.Instantiate(Projectile);
                        projectile.transform.position = FirePoint.transform.position;
                        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
                        projectileController.direction = (monster.transform.position - this.transform.position).normalized;
                        projectileController.atk = this.atk;
                    }

                    this.cd = 1 / this.speed;
                    break;
                }
            }
        }
    }

    protected override void die () {

    }
}
