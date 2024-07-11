using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRange : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    private float rate;
    private float Radius;

    public void Setup(Transform target, float damage, float rate, float Radius)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
        this.rate = rate;
        this.Radius = Radius;
    }

    private void Update()
    {
        if (target != null)
        {
            // 발사체를 target의 위치로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;             // 적이 아닌 대상과 부딪히면
        if (collision.transform != target) return;              // 현재 target인 적이 아닐 때
        Explode();
        Destroy(gameObject);
    }
    
    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyHP>().TakeDamage(damage);   // 적 체력을 damage만큼 감소
            }
        }
    }
}
