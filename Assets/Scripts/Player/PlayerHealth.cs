using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int playerNumber;
    public int currHealth;
    public AudioSource death;
    public AudioSource hit;

    public bool dead = false;
    [SerializeField] float ghostYSpeed;

    public Action playerDamagedAction;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        GameUI.Instance.UpdateHealthUI(playerNumber, currHealth);
    }

    public void AddHealth(int amount)
    {
        currHealth += amount;
        GameUI.Instance.UpdateHealthUI(playerNumber, currHealth);
    }

    public void RemoveHealth(int amount)
    {

        if (currHealth > 0)
        {
            if (currHealth >= amount)
            {
                currHealth -= amount;
                hit.Play();
                GameUI.Instance.UpdateHealthUI(playerNumber, currHealth);
                if (currHealth <= 0)
                {
                    Die();
                }
                playerDamagedAction.Invoke();
            }
        }
    }

    public void Die()
    {
        currHealth = 0;
        GameUI.Instance.UpdateHealthUI(playerNumber, currHealth);
        GameManager.instance.PlayerDied(playerNumber);
        GetComponent<Animator>().SetBool("Death", true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<CharacterController2D>().canMove = false;
        GetComponent<PickUpItems>().DeleteItemFromHands();
        death.Play();
        dead = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("death"))
        {
            Die();
        }
    }

    private void Update()
    {
        if (dead)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + ghostYSpeed * Time.deltaTime);
        }
    }
}
