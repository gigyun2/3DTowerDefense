using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : AttackableController {

    override public void Start () {
        base.Start();
		this.tag = "Barrier";		

		// every level increase hp 10%, def 20%
		int level = 1;
		if (PlayerPrefs.HasKey ("Barrier1")) {
			PlayerPrefs.GetInt ("Barrier1");
		}
		this.hp = (int)(500 * (1 + 0.1 * (level - 1)));
		this.def = (int)(10 * (1 + 0.2 * (level - 1)));
    }

    override public void Update () {
        base.Update();
    }

    void OnCollisionEnter (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().speed *= 1/2;
        }
    }

    void OnCollisionEnd (Collision collision) {
        GameObject collidedObject = collision.gameObject.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().speed *= 2;
        }
    }

    protected override void die () {

    }

}
