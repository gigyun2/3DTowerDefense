using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : AttackableController {
	public List<Vector3> route;
	public int reward;

	void Start () {
		base.Start ();
		this.tag = "Monster";
		this.route = new List<Vector3>();
	}
		
	void Update () {
		base.Update ();
		if (route.Count > 0) {
			Vector3 direction = (route [0] - this.transform.position);
			direction.y = 0;
			direction = direction.normalized;
			this.GetComponent<Rigidbody> ().velocity = direction * 1.5f;
		}  else {
			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}

	}

	void LateUpdate () {
		if (route.Count > 0) {
			Vector3 temp = this.transform.position;
			temp.y = 0;
			if (Vector3.Distance (temp, route [0]) < 0.1f) {
				route.RemoveAt (0);
			}
		}
	}

	protected override void die() {
		// give reawrd to palyer
	}
}
