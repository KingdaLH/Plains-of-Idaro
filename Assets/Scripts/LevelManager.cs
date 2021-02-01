using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public static LevelManager instance;
    public Vector2 spawnPos;

    private void Start()
    {
        instance = this;
        spawnPos = GameObject.Find("SpawnPos").transform.position;
        GameManager.instance.player.transform.position = spawnPos;
    }
}
