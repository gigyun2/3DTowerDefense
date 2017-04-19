using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image=UnityEngine.UI.Image;

public class HPController : MonoBehaviour {
	Image Healthbar;
	float tmpHealth=1;

	// Use this for initialization
	void Start () {
		Healthbar=GameObject.Find ("FirstPersonController").transform.FindChild ("Canvas").FindChild("health-bar").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		Healthbar.fillAmount = tmpHealth;

	}
	public void changeHealth(float addHealth, float subHealth){
		//call changeHealth to adust health rate
		tmpHealth = tmpHealth+addHealth-subHealth;
		if (tmpHealth > 1f)
			tmpHealth = 1f;
		//if (tmpHealth == 0f)
			//endGame();

	}
}
