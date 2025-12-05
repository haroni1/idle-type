using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHp = 1;

    private int currentHp;
    private Transform player;

    private void Start()
    {
        currentHp = maxHp;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.AddChicken(1);
        Destroy(gameObject);
    }
}
