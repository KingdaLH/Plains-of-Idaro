using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public AudioSource rip;

    public static IEnumerator DeathSequencePlane()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.Respawn();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerScript.isDead = true;
            print("test");
            rip.Play();
            StartCoroutine(DeathSequencePlane());
        }
    }
}
    