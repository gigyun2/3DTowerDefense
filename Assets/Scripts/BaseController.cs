using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : AttackableController {

	// Use this for initialization
	override public void Start () {
        base.Start();
        this.tag = "Base";
    }

    // Update is called once per frame
    override public void Update () {
        base.Update();
	}

    protected override void die () {
        Debug.Log("Stage Failed!");
		GameObject.Find ("Canvas").GetComponent<UIController> ().onLose ();
    }
}
