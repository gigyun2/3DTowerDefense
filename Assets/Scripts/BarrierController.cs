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
		this.hp = (int)(50 * (1 + 0.1 * (level - 1)));
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
        
        GameObject damageText = new GameObject();
        damageText.name = "Damage Text";
        TextMesh textMesh = damageText.AddComponent<TextMesh>();
        textMesh.text = "Destroyed!";
        textMesh.fontSize = 7;
        textMesh.anchor = TextAnchor.MiddleCenter;
        damageText.transform.SetParent(null);
        damageText.transform.localPosition = new Vector3(0, 0.1f, 0);
        damageText.transform.rotation = Camera.main.transform.rotation;
        GameObject.Destroy(damageText, 2f);

        Destroy(this.gameObject);
    }

}
