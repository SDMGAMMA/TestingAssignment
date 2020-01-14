using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float health;
    [SerializeField] int damage;

    [SerializeField] GameObject bar;

    void FixedUpdate()
    {
        bar.gameObject.transform.localScale = new Vector2(health, 20);
    }

    public void Damage()
    {
        health -= damage;
    }
}
