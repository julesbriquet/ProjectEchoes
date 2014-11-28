using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour {

    public int health;

	public virtual void TakeDamage (int damage) {

        health -= damage;

        if (health < 0)
            Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
