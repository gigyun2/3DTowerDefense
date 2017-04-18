using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : AttackableController {
	public string type;
	void Start () {
		base.Start ();
		this.tag = "Trap";
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collidedObject = collision.gameObject.transform.root.gameObject;
		if (collidedObject != null && collidedObject.tag.Equals ("Monster")) {
			if (this.type.Equals ("single")) {
				collidedObject.GetComponent<MonsterController> ().Hurt (this.atk);
			} else if (this.type.Equals ("range")) {
				foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
					if (Vector3.Distance (monster.transform.position, this.transform.position) < this.range) {
						monster.GetComponent<MonsterController> ().Hurt (this.atk);
					}
				}
			}
            // TODO: with explose particle effect?
            die();
        }
    }

    protected override void die()
    {
        GameObject.Destroy(this);
    }
}
