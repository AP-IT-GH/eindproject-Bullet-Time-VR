using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //gameObject.GetComponent<Animator>().Play("uw animatie (komt hier)");
        Destroy(gameObject, 1f);
    }
}