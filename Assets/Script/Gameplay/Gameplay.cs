using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    float timer = 0;
    bool stab = false;
    float timeToWait = 0;
    bool timerDone = false;
    static float facing = 1;
    bool checkingTime = false;
    Vector3 move = Vector3.zero;
    new Animator animation = null;
    [SerializeField] private int speed = 5;
    [SerializeField] private BoxCollider2D boxR;
    [SerializeField] private BoxCollider2D boxL;
    public AudioClip[] stings;
    public AudioSource stingSource;

    void Start()
    {
        animation = GetComponent<Animator>();
    }

    void PlayerInput()
    {
        move = Vector3.zero;
        if (Input.GetKey(KeyCode.Space))
        {
            move += new Vector3(0, 1.5f, 0);
            animation.SetBool("jumping", true);
            animation.SetBool("walking", false);
            animation.SetBool("attacking", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += new Vector3(1, 0, 0);
            facing = 1;
            animation.SetFloat("x", 1);
            animation.SetBool("jumping", false);
            animation.SetBool("walking", true);
            animation.SetBool("attacking", false);
        }
        else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
        {
            move += new Vector3(-1, 0, 0);
            facing = -1;
            animation.SetFloat("x", -1);
            animation.SetBool("jumping", false);
            animation.SetBool("walking", true);
            animation.SetBool("attacking", false);
        }
        else
        {
            animation.SetFloat("x", facing);
            animation.SetBool("jumping", false);
            animation.SetBool("walking", false);
            animation.SetBool("attacking", false);
        }
        gameObject.transform.position += move * speed * Time.deltaTime;
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
                timer = 0;
            }
        }
        if (timerDone)
        {
            timerDone = false;
            stab = false;
            boxL.enabled = false;
            boxR.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            timeToWait = 1f;
            animation.SetFloat("x", facing);
            animation.SetBool("walking", false);
            animation.SetBool("attacking", true);
            checkingTime = true;
            if (facing == -1)
                boxL.enabled = true;
            else
                boxR.enabled = true;
            stab = true;
        }
        Timer();
        if (!checkingTime)
            PlayerInput();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "IA" && stab == true)
        {
            collision.gameObject.GetComponent<IaMove>().SetLife(1);
            stingSource.clip = stings[0];
            stingSource.Play();
            timerDone = false;
            stab = false;
            boxL.enabled = false;
        }
    }

    public bool GetStab()
    {
        return stab;
    }
}
