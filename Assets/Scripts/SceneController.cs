﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
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
        int n = 0;
        while (n < number) {
            List<Vector3> monsterRoute = new List<Vector3>(route);
            for (int i = 0; i < monsterRoute.Count; i++) {
                monsterRoute[i] = monsterRoute[i] * 2 + new Vector3(Random.value*1.6f-0.8f, 0, Random.value* 1.6f - 0.8f);
            }

            GameObject spawn = Instantiate(monster, monsterRoute[0], Quaternion.identity);
            spawn.GetComponent<MonsterController>().route = monsterRoute;
            monsters.Add(spawn);

            n++;
            yield return new WaitForSeconds(1f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}