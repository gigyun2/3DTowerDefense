using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AttackableController : MonoBehaviour {

    // TODO: Needs check whether public or protected
    public int hp;
    public int atk;
    public int def;
    public float range;
    public float speed;
    public float cd;
    public bool isAlive;

	// Use this for initialization
	public virtual void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (this.cd > 0) {
			this.cd -= Time.deltaTime;
		}

		// updating Damage Text Visual effect
		for (int i = 0; i < this.transform.childCount; i++) {
			GameObject child = this.transform.GetChild (i);
			if (child.name.Equals ("Damage Text")) {
				child.transform.position = child.transform.position + new Vector3 (0, 0.1f, 0);
				// TODO: this rotation should be wrong, wait for test.
				child.transform.rotation = Camera.main.transform.rotation;
			}
		}

		if (!isAlive) {
            //GetComponent<MeshRenderer>().material.color.a = Mathf.Lerp(1, 0, t);
            //t += Time.deltaTime * (1.0f / 2);
            //if (t > 1.0f) {
            //    Destroy(gameObject);
            //}
        }
	}

    abstract protected void OnCollisionEnter(Collision collision);

    /// not decided return type
    public void Hurt (int damage) {
        if (isAlive) {
			int effectiveDamage = (damage - this.def);
			if (effectiveDamage <= 1)
				effectiveDamage = 1;
			
			this.hp -= effectiveDamage;
            if (this.hp <= 0) {
                this.isAlive = true;
                die();
            }

			GameObject damageText = new GameObject ();
			damageText.name = "Damage Text";
			TextMesh textMesh = damageText.AddComponent<TextMesh>();
			textMesh.text = effectiveDamage;
			textMesh.fontSize = 8;
			textMesh.anchor = TextAnchor.MiddleCenter;
			damageText.transform.SetParent (this.transform);
			damageText.transform.localPosition = new Vector3 (0, 0, 0);
			GameObject.Destroy (damageText, 2f);
        }
    }

    /// not decided return/param type
    abstract protected void die();
}
