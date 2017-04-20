using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Image=UnityEngine.UI.Image;

public class HPController : MonoBehaviour {
	Image Healthbar;
	float tmpHealth=1;

    FirstPersonController playerController;

	// Use this for initialization
	void Start () {
        playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        Healthbar = this.transform.FindChild("health-bar").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		Healthbar.fillAmount = playerController.hp/100f;

	}
	/*public void changeHealth(float addHealth, float subHealth){
		//call changeHealth to adust health rate
		tmpHealth = tmpHealth+addHealth-subHealth;
		if (tmpHealth > 1f)
			tmpHealth = 1f;
		//if (tmpHealth == 0f)
			//endGame();

	}*/
}
