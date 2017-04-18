using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : AttackableController {
	public GameObject FirePoint;
	public GameObject Projectile; //Prefabs of projectile to shoot

	void Start () {
		base.Start ();
		this.tag = "Tower";
	}

	void Update () {
		base.Update ();
		if (cd <= 0) {
			foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
				if (Vector3.Distance (monster.transform.position, this.transform.position) < this.range) {
					// face to target
					Quaternion quaternion = Quaternion.LookRotation (monster.transform.position - this.transform.position);
					quaternion.x = 0;
					quaternion.z = 0;
					this.transform.rotation = quaternion;

					// shoot
					if (Projectile != null) {
						GameObject projectile = GameObject.Instantiate (Projectile);
						projectile.transform.position = FirePoint.transform.position;
						ProjectileController projectileController = projectile.GetComponent<ProjectileController> ();
						projectileController.direction = (monster.transform.position - this.transform.position).normalized;
						projectileController.atk = this.atk;
					}

					this.cd = 1 / this.speed;
					break;
				}
			}
		}
	}
}
