﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public float time;
    public int reward;
    private List<GameObject> monsters;
    private List<Vector3> route;
    private GameObject monster; // TODO: add various type of monsters

	// Use this for initialization
	void Start () {
        monsters = new List<GameObject>();
        monster = (GameObject)Resources.Load("Monster");

		route = new List<Vector3>{new Vector3(5,0,2), new Vector3(4,0,2), new Vector3(4,0,3),
            new Vector3(4,0,4), new Vector3(3,0,4), new Vector3(2,0,4), new Vector3(1,0,4), new Vector3(0,0,4),
            new Vector3(0,0,3), new Vector3(0,0,2), new Vector3(0,0,1), new Vector3(0,0,0), new Vector3(1,0,0),
            new Vector3(2,0,0), new Vector3(2,0,1), new Vector3(2,0,2)};

        StartCoroutine(SpawnMonster(10));
	}

    IEnumerator SpawnMonster(int number) {
        yield return new WaitForSeconds(1f);

        int n = 0;
        while (n < number) {
            List<Vector3> monsterRoute = new List<Vector3>(route);
            for (int i = 0; i < monsterRoute.Count; i++) {
                monsterRoute[i] = monsterRoute[i] * 2 + new Vector3(Random.value*1.2f-0.6f, 0, Random.value*1.2f-0.6f);
            }
            GameObject spawn = Instantiate(monster, monsterRoute[0], Quaternion.identity);
            monsterRoute.RemoveAt(0);
            spawn.GetComponent<MonsterController>().route = monsterRoute;
            monsters.Add(spawn);

            n++;
            yield return new WaitForSeconds(1f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < monsters.Count; i++) {
            if (!monsters[i].GetComponent<MonsterController>().isAlive) {
                monsters.RemoveAt(i);
            }
        }
        if (Time.timeSinceLevelLoad > 11 && monsters.Count == 0) {
            UIController ui = GameObject.Find("Canvas").GetComponent<UIController>();
            ui.onWin();
            Destroy(this);
        } else if (time < Time.timeSinceLevelLoad) {
            UIController ui = GameObject.Find("Canvas").GetComponent<UIController>();
            ui.onLose();
            Destroy(this);
        }
	}
}