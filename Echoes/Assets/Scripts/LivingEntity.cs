using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour {

    public int maxHealth;
    private int health;

    void Start()
    {
        health = maxHealth;
    }

	public virtual void TakeDamage (int damage) {

        health -= damage;

        if (health < 0)
            Die();
    }

    public virtual void GetHeal(int healPoint)
    {
        if (health + healPoint > maxHealth)
            health = maxHealth;
        else
            health += healPoint;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
