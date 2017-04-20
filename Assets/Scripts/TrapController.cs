using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : AttackableController {
	public string type;

    override public void Start() {
        base.Start();
		this.tag = "Trap";

		// every level increase atk 50%, range 0.5
		int level = 1;
		if (PlayerPrefs.HasKey ("Trap1")) {
			PlayerPrefs.GetInt ("Trap1");
		}
		this.atk = (int)(10 * (1 + 0.5 * (level - 1)));
		this.range = (float)(3 + 0.5 * (level - 1));
	}
	
	// Update is called once per frame
	override public void Update () {
		base.Update ();
	}

	void OnCollisionStay(Collision collision) {
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

    protected override void die() {
        GameObject.Destroy(this);
    }
}
