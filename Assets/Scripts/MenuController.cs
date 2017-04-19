using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public GameObject mainPanel;
	public GameObject levelPanel;
	public GameObject shopPanel;

	const int TOWER_AMOUNT = 1;
	const int BARRIER_AMOUNT = 1;
	const int TRAP_AMOUNT = 1;
	const int WEAPON_AMOUNT = 1;

	void Start() {
		LoadProgress();
		LoadShop ();


		gotoMainPanel ();
	}

	void LoadProgress() {
		// initialize level setting
		int progress = 1;
		if (PlayerPrefs.HasKey ("Progress"))
			progress = PlayerPrefs.GetInt ("Progress");
		for (int i = 1; i <= progress; i++) {
			GameObject.Find ("Level Button (" + i + ")").GetComponent<Button> ().interactable = true;
		}
	}

	void LoadShop() {
		// initialize money
		int money = 0;
		if (PlayerPrefs.HasKey ("Money"))
			money = PlayerPrefs.GetInt ("Money");
		GameObject.Find ("Money").GetComponent<Text> ().text = "$ " + money;

		// initialize tower setting
		for (int i = 1; i <= TOWER_AMOUNT; i++) {
			int level = 0;
			if (PlayerPrefs.HasKey ("Tower" + i))
				level = PlayerPrefs.GetInt ("Tower" + i);
			GameObject tower = GameObject.Find ("Tower (" + i + ")");
			if (level == 0) {
				tower.GetComponentInChildren<Text> ().text = "Unlock($1000)";
				tower.GetComponentInChildren<Button> ().interactable = (money >= 1000);
			} else if (level == 10) {
				tower.GetComponentInChildren<Text> ().text = "Max.";
				tower.GetComponentInChildren<Button> ().interactable = false;
			} else {
				tower.GetComponentInChildren<Text> ().text = "Upgrade($" + (300 * level) + ")";
				tower.GetComponentInChildren<Button> ().interactable = (money >= level * 300);
			}
		}

		// initialize Barrier setting
		for (int i = 1; i <= BARRIER_AMOUNT; i++) {
			int level = 0;
			if (PlayerPrefs.HasKey ("Barrier" + i))
				level = PlayerPrefs.GetInt ("Barrier" + i);
			GameObject barrier = GameObject.Find ("Barrier (" + i + ")");
			if (level == 0) {
				barrier.GetComponentInChildren<Text> ().text = "Unlock($800)";
				barrier.GetComponentInChildren<Button> ().interactable = (money >= 800);
			} else if (level == 10) {
				barrier.GetComponentInChildren<Text> ().text = "Max.";
				barrier.GetComponentInChildren<Button> ().interactable = false;
			} else {
				barrier.GetComponentInChildren<Text> ().text = "Upgrade($" + (200 * level) + ")";
				barrier.GetComponentInChildren<Button> ().interactable = (money >= level * 200);
			}
		}

		// initialize Trap setting
		for (int i = 1; i <= TRAP_AMOUNT; i++) {
			int level = 0;
			if (PlayerPrefs.HasKey ("Trap" + i))
				level = PlayerPrefs.GetInt ("Trap" + i);
			GameObject trap = GameObject.Find ("Trap (" + i + ")");
			if (level == 0) {
				trap.GetComponentInChildren<Text> ().text = "Unlock($500)";
				trap.GetComponentInChildren<Button> ().interactable = (money >= 500);
			} else if (level == 10) {
				trap.GetComponentInChildren<Text> ().text = "Max.";
				trap.GetComponentInChildren<Button> ().interactable = false;
			} else {
				trap.GetComponentInChildren<Text> ().text = "Upgrade($" + (150 * level) + ")";
				trap.GetComponentInChildren<Button> ().interactable = (money >= level * 150);
			}
		}

		// initialize Weapon setting
		for (int i = 1; i <= WEAPON_AMOUNT; i++) {
			int level = 0;
			if (PlayerPrefs.HasKey ("Weapon" + i))
				level = PlayerPrefs.GetInt ("Weapon" + i);
			GameObject weapon = GameObject.Find ("Weapon (" + i + ")");
			if (level == 0) {
				weapon.GetComponentInChildren<Text> ().text = "Unlock($1500)";
				weapon.GetComponentInChildren<Button> ().interactable = (money >= 1500);
			} else if (level == 10) {
				weapon.GetComponentInChildren<Text> ().text = "Max.";
				weapon.GetComponentInChildren<Button> ().interactable = false;
			} else {
				weapon.GetComponentInChildren<Text> ().text = "Upgrade($" + (450 * level) + ")";
				weapon.GetComponentInChildren<Button> ().interactable = (money >= level * 450);
			}
		}
	}


	public void gotoLevelPanel() {
		mainPanel.SetActive (false);
		levelPanel.SetActive (true);
		shopPanel.SetActive (false);
	}

	public void gotoMainPanel() {
		mainPanel.SetActive (true);
		levelPanel.SetActive (false);
		shopPanel.SetActive (false);
	}

	public void gotoSettingPanel() {
		mainPanel.SetActive (false);
		levelPanel.SetActive (false);
		shopPanel.SetActive (true);
	}

	public void exit() {
		Application.Quit ();
	}

	public void selectLevel (int level) {
		SceneManager.LoadScene ("level" + level);
	}

	public void upgradeTower (int index) {
		int money = 0;
		if (PlayerPrefs.HasKey ("Money"))
			money = PlayerPrefs.GetInt ("Money");

		int level = 0;
		if (PlayerPrefs.HasKey ("Tower" + index))
			level = PlayerPrefs.GetInt ("Tower" + index);

		int cost = level == 0 ? 1000 : level < 10 ? level * 300 : 99999;
		if (money >= cost) {
			money -= cost;
			level++;
			PlayerPrefs.SetInt ("Money", money);
			PlayerPrefs.SetInt ("Tower" + index, level);
			LoadShop ();
		}
	}

	public void upgradeBarrier (int index) {
		int money = 0;
		if (PlayerPrefs.HasKey ("Money"))
			money = PlayerPrefs.GetInt ("Money");

		int level = 0;
		if (PlayerPrefs.HasKey ("Barrier" + index))
			level = PlayerPrefs.GetInt ("Barrier" + index);

		int cost = level == 0 ? 800 : level < 10 ? level * 200 : 99999;
		if (money >= cost) {
			money -= cost;
			level++;
			PlayerPrefs.SetInt ("Money", money);
			PlayerPrefs.SetInt ("Barrier" + index, level);
			LoadShop ();
		}
	}

	public void upgradeTrap (int index) {
		int money = 0;
		if (PlayerPrefs.HasKey ("Money"))
			money = PlayerPrefs.GetInt ("Money");

		int level = 0;
		if (PlayerPrefs.HasKey ("Trap" + index))
			level = PlayerPrefs.GetInt ("Trap" + index);

		int cost = level == 0 ? 500 : level < 10 ? level * 150 : 99999;
		if (money >= cost) {
			money -= cost;
			level++;
			PlayerPrefs.SetInt ("Money", money);
			PlayerPrefs.SetInt ("Trap" + index, level);
			LoadShop ();
		}
	}

	public void upgradeWeapon (int index) {
		int money = 0;
		if (PlayerPrefs.HasKey ("Money"))
			money = PlayerPrefs.GetInt ("Money");

		int level = 0;
		if (PlayerPrefs.HasKey ("Weapon" + index))
			level = PlayerPrefs.GetInt ("Weapon" + index);

		int cost = level == 0 ? 1500 : level < 10 ? level * 450 : 99999;
		if (money >= cost) {
			money -= cost;
			level++;
			PlayerPrefs.SetInt ("Money", money);
			PlayerPrefs.SetInt ("Weapon" + index, level);
			LoadShop ();
		}
	}
}
