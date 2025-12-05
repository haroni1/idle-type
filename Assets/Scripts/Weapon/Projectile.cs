using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifeTime = 3f;

    private Transform target;

    public void Init(Transform target, float speed, int damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, lifeTime); // 일정 시간 뒤 자동 삭제
    }

    private void Update()
    {
        if (target == null)
        {
            // 타겟이 죽었으면 그냥 직진하거나 삭제
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Chicken 레이어 / 컴포넌트 체크
        Chicken chicken = other.GetComponent<Chicken>();
        if (chicken != null)
        {
            chicken.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
