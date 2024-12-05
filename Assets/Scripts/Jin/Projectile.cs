using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private bool isLookAt;
    [SerializeField]
    private int effect_num;
    [SerializeField]
    private AudioClip _hitSFX;

    private Movement2D movement2D;
    private Transform target;
    private float damage;
    private float rate;
    public void Setup(Transform target, float damage, float rate)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;                       // 타워가 설정해준 target
        this.damage = damage;                       // 타워가 설정해준 공격력
        this.rate = rate;
    }
    private void Update()
    {
        if(target != null)
        {
            // 발사체를 target의 위치로 이동
            Vector2 direction = (target.position - transform.position).normalized;
            
            if(isLookAt)
            {
                MyLookat(direction);
            }
            
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

        collision.GetComponent<EnemyHP>().TakeDamage(damage);   // 적 체력을 damage만큼 감소
        collision.GetComponent<EnemyHP>().EffectOn(effect_num); // 이펙트 생성
        SFX_Manager.instance.Play_oneshot(_hitSFX);           // 피격시 효과음 재생
        Destroy(gameObject);
    }

    private void MyLookat(Vector2 dir)
    {
        float radian = Mathf.Atan2(dir.y, dir.x);

        float angle = radian * 180 / Mathf.PI;

        transform.rotation = Quaternion.Euler(0,0,angle - 90.0f);
    }
}
