using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaMove : MonoBehaviour
{
    new Animator animation = null;
    GameObject Player = null;
    float speed = 0.5f;
    int life = 1;
    int old = 0;
    float timer = 0;
    float timeToWait = 1.1f;
    bool timerDone = false;
    bool checkingTime = true;


    void Start()
    {
        animation = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Timer()
    {
        if (checkingTime)
        {
            timer += Time.deltaTime;
            if (timer >= timeToWait)
            {
                timerDone = true;
                checkingTime = false;
            }
        }
        if (timerDone)
        {
            timerDone = false;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        Vector3 oldPos = transform.position;
        if (distance > 0.2f)
        {
            if (distance < 1f)
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
            if (oldPos.x < transform.position.x)
            {
                old = 1;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                old = 1;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (life <= 0) {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            animation.SetFloat("x", old);
            animation.SetBool("dead", true);
 
            Timer();
        }   
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public void SetLife(int life)
    {
        this.life -= life;
    }

}
