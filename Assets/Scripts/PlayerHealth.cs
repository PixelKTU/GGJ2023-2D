using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    public void AddHealth(int amount)
    {
        currHealth += amount;
    }
    
    public void RemoveHealth(int amount)
    {

        if (currHealth > 0)
        {
            if (currHealth >= amount)
            {
                currHealth -= amount;
            }
            else
            {
                currHealth = 0;
            }
        }
    }
}
