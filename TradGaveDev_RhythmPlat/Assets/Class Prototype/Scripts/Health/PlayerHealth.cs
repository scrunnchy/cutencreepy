using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;

    public UnityEvent damageEvent;
    public UnityEvent deathEvent;

    private int _currentHealth;
    private bool _canTakeDamage = true;

    public void Start ()
    {
        _currentHealth = startingHealth;
    }

    public void DealDamage (int damage)
    {
        if (!_canTakeDamage) return;

        _currentHealth -= damage;

        print(this.gameObject.name + " HP = " + _currentHealth);

        damageEvent.Invoke();

        if (_currentHealth <= 0)
        {
            PlayerDeath();
            _currentHealth = 0;
        }
    }

    public void PlayerDeath ()
    {
        print(this.gameObject.name + " Died");

        deathEvent.Invoke();
        _canTakeDamage = false;
    }
}
