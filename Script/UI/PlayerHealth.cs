using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image currentHealth;
    public Image delayHealth;
    public float bleedSpeed=1;

    private void Update()
    {
        if(delayHealth.fillAmount>currentHealth.fillAmount)
        {
            delayHealth.fillAmount -= Time.deltaTime*bleedSpeed;   
        }
    }

    public void HealthChange(float percentage)
    {
        currentHealth.fillAmount = percentage;
    }
}
