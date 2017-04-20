using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Image = UnityEngine.UI.Image;
using Text = UnityEngine.UI.Text;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	Image Healthbar;
    Text time;
    Text money;
    bool playing;
	public GameObject EndPanel;
	public GameObject WinPanel;
	public GameObject LosePanel;

    SceneController sceneController;
    FirstPersonController playerController;

	void Start () {
        sceneController = GameObject.Find("LevelConfig").GetComponent<SceneController>();
        playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        Healthbar = this.transform.FindChild("HP").FindChild("health-bar").GetComponent<Image>();
        time = this.transform.FindChild("Time").FindChild("Time").GetComponent<Text>();
        money = this.transform.FindChild("Money").FindChild("Money").GetComponent<Text>();
        playing = true;
    }

	void Update () {
        if (playing) {
            Healthbar.fillAmount = playerController.hp / 100f;
            time.text = (sceneController.time - Time.time).ToString();
            money.text = (playerController.money).ToString();
        }
    }

	public void onWin() {
        playing = false;
		playerController.enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		EndPanel.SetActive (true);
		WinPanel.SetActive (true);

		int level = int.Parse(SceneManager.GetActiveScene ().name.Substring(5));
		PlayerPrefs.SetInt("Progress", (level));
        int money = PlayerPrefs.GetInt("Money", 0);
        money = money + playerController.money + 1000;
        PlayerPrefs.SetInt("Money", money);
	}

	public void onLose() {
        playing = false;
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
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void backToMenu() {
		SceneManager.LoadScene ("Menu");
	}
}
