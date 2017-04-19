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

    void OnTriggerEnter (Collider collider) {
        GameObject collidedObject = collider.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().velocity *= 0.5f;
        }
    }

    void OnTriggerEnd (Collider collider) {
        GameObject collidedObject = collider.transform.root.gameObject;
        if (collidedObject != null && collidedObject.tag.Equals("Monster")) {
            collidedObject.GetComponent<MonsterController>().velocity *= 2;
        }
    }

    protected override void die () {

    }

}
