using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int damage;
    public GameObject Enemy;
    public Animator animator;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer += Time.deltaTime;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Dying();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            damage++;
            animator.SetBool("isHit", true);
            Invoke("BackToNormal", 1.5f);
            Debug.Log("Hit enemy!");
        }
    }

    private void BackToNormal()
    {
        animator.SetBool("isChasing", true);
        animator.SetBool("isHit", false);
    }
    public void Dying()
    {
        if(damage >= 5)
        {
            Debug.Log("Dead!");
            GetComponent<AIController>().enabled = false;
            animator.SetBool("isDead", true);
            animator.SetBool("isChasing", false);
            Invoke("isDead", 5);
        }
    }

    public void isDead()
    {
        Enemy.SetActive(false);
    }
}
