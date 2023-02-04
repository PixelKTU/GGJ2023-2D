using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int playerNumber;
    public int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        GameManager.instance.UpdateHealthUI(playerNumber, currHealth);
    }

    public void AddHealth(int amount)
    {
        currHealth += amount;
        GameManager.instance.UpdateHealthUI(playerNumber, currHealth);
    }
    
    public void RemoveHealth(int amount)
    {

        if (currHealth > 0)
        {
            if (currHealth >= amount)
            {
                currHealth -= amount;
                GameManager.instance.UpdateHealthUI(playerNumber, currHealth);
                if (currHealth <= 0)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("death"))
        {
            Die();
        }
    }
}
