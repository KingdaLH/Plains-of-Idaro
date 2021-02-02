using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int scoreWorth;
    public AudioSource aS;
    public AudioSource aST;
    public Vector2 TP;

    private void Start()
    {
        aS = GameObject.Find("CoinAudio").GetComponent<AudioSource>();
        if (GameManager.instance.currentLevel == 1)
            aST = GameObject.Find("EpicAudio").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && this.CompareTag("TP"))
        {
            aST.Play();
            col.transform.position = TP;
            Collect();
        }
        else if (col.CompareTag("Player") && !this.CompareTag("TP"))
        {
            Collect();
            aS.Play();
        }
        
    }

    private void Collect()
    {
        GameManager.instance.score += scoreWorth;
        GameManager.instance.ScoreChange();
        Destroy(gameObject);
    }
}
