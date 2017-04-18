using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : AttackableController {

	void Start () {
		base.Start ();
		this.tag = "Barrier";
	}

	void Update () {
		base.Update ();
	}

    protected override void die()
    {

    }

}
