using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pos1, pos2;
    public Transform startPos;
    public float speed;
    private Vector3 nextPos;
    private GameObject target;
    private Vector3 offset;
    void Start()
    {
        nextPos = startPos.position;
        target = null;
    }
    
   // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Vector2.Distance(transform.position, pos1.transform.position));
        if (Vector2.Distance(transform.position, pos1.transform.position) < 0.1)
        {
            nextPos = pos2.position;
            //transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        if (Vector2.Distance(transform.position, pos2.transform.position) < 0.1)
        {
            nextPos = pos1.position;
            //transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        
        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
    
    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            target = col.gameObject;
            offset = target.transform.position - transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            target = null;
        }
    }
}
