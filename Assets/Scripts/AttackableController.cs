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
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
            hp -= damage;
            if (this.hp <= 0) {
                isAlive = true;
                die();
            }
        }
    }

    /// not decided return/param type
    abstract protected void die();

}
