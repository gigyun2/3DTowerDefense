using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Image=UnityEngine.UI.Image;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	Image Healthbar;
	public GameObject EndPanel;
	public GameObject WinPanel;
	public GameObject LosePanel;

    FirstPersonController playerController;

	void Start () {
        playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
		Healthbar = this.transform.FindChild("HP").FindChild("health-bar").GetComponent<Image>();
	}

	void Update () {
		Healthbar.fillAmount = playerController.hp/100f;
	}

	public void onWin() {
		playerController.enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		EndPanel.SetActive (true);
		WinPanel.SetActive (true);

		int level = int.Parse(SceneManager.GetActiveScene ().name.Substring(5));
		PlayerPrefs.SetInt("Progress", (level));
	}

	public void onLose() {
		playerController.enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		EndPanel.SetActive (true);
		LosePanel.SetActive (true);
	}

	public void nextLevel() {
		int level = int.Parse(SceneManager.GetActiveScene ().name.Substring(5));
		SceneManager.LoadScene ("Stage" + (level + 1));
	}

	public void retry() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name.Substring(5));
	}

	public void backToMenu() {
		SceneManager.LoadScene ("Menu");
	}
}
