using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0, Laser, Slow, Buff, Debuff, RangeAttack}
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser, TryRangeAttack}
public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private Animator        _Animator;                              // 애니메이터
    [SerializeField]
    private TowerTemplate   towerTemplate;                          // 타워 정보(공격력, 공격속도 등)
    [SerializeField]
    private Transform       spawnPoint;                             // 발사체 생성 위치
    [SerializeField]
    private WeaponType      weaponType;                             // 무기 속성 설정
    public bool Is_Selected;

    [Header("Cannon")]
    [SerializeField]
    private GameObject      projectilePrefab;                       // 발사체 프리팹    

    [Header("Laser")]
    [SerializeField]
    private LineRenderer    lineRenderer;
    [SerializeField]
    private Transform       hitEffect;                              // 타격 효과
    [SerializeField]
    private LayerMask       targetLayer;                            // 광선에 부딪히는 레이어 설정
    [SerializeField]
    private float           damageIncreaseRate;

    [Header("RangeAttack")]
    [SerializeField]
    private GameObject projectileRangePrefab;
    [SerializeField]
    private LayerMask targetLayerRange;
    [SerializeField]
    private float Radius;

    private int             level = 0;
    private WeaponState     weaponState = WeaponState.SearchTarget; // 타워 무기의 상태
    private Transform       attackTarget = null;                    // 공격 대상
    private SpriteRenderer  spriteRenderer;
    private TowerSpawner    towerSpawner;
    private EnemySpawner    enemySpawner;                           // 게임에 존재하는 적 정보 획득용
    private PlayerGold      playerGold;
    private Tile            ownerTile;

    private float buffRate;
    private int buffLevel;

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    public float Debuff => towerTemplate.weapon[level].debuff;
    public int TowerType => towerTemplate.towerType;

    public float BuffRate
    {
        set => buffRate = Mathf.Max(0, value);
        get => buffRate;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    public WeaponType WeaponType => weaponType;
    
    public void Setup(TowerSpawner towerSpawner, EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        //등급에 따른 애니메이션 트리거 세팅
        _Animator.SetBool("Level1",true);

        if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser || weaponType == WeaponType.RangeAttack)
        {
            // 최초 상태를 WeaponState.SearchTarget으로 설정
            ChangeState(WeaponState.SearchTarget);
        }
        
    }
    public void ChangeState(WeaponState newState)
    {
        // 이전에 재생중이던 상태 종료
        StopCoroutine(weaponState.ToString());
        // 상태 변경
        weaponState = newState;
        // 새로운 상태 재생
        StartCoroutine(weaponState.ToString());
    }
    private void Update()
    {
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }
    private void RotateToTarget()
    {
        // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용
        // 각도 = arctan(y/x)
        // x, y 변위값 구하기
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // x, y 변위값을 바탕으로 각도 구하기
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 두 단위를 구함
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, degree);
        RotateImage(degree);
    }

    private void RotateImage(float angle)
    {
        // 왼쪽
        if (Mathf.Abs(angle) > 135)
        {
            _Animator.SetTrigger("Left");
        }
        // 오른쪽
        else if (Mathf.Abs(angle) < 45)
        {
            _Animator.SetTrigger("Right");
        }
        // 위
        else if (angle > 45 && angle < 135)
        {
            _Animator.SetTrigger("Up");
        }
        // 아래
        else if (angle < -45 && angle > -135)
        {
            _Animator.SetTrigger("Down");
        }
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            // 현재 타워에 가장 가까이 있는 공격 대상 탐색
            attackTarget = FindClosestAttackTarget();
            if (attackTarget != null)
            {
                if(weaponType == WeaponType.Cannon || weaponType == WeaponType.RangeAttack)
                {
                    ChangeState(WeaponState.TryAttackCannon);

                }
                else if(weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }
            yield return null;
        }
    }
    private Transform FindClosestAttackTarget()
    {

        // 제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
        float closestDistSqr = Mathf.Infinity;
        // EnemySpawner의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {

            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            // 현재 검사중인 적과의 거리가 공격범위 내에 있고, 현재까지 검사한 적보다 거리가 가까우면
            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }
        return attackTarget;
    }
    private IEnumerator TryAttackCannon()
    {
        float rate = towerTemplate.weapon[level].rate - BuffRate;
        while (true)
        {
            // target을 공격하는게 가능한지 검사
            if(IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(rate);
            // 공격 (발사체 생성)
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        EnableLaser();

        while (true)
        {
            // target을 공격하는게 가능한지 검사
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 레이저 공격
            SpawnLaser();
            yield return null;
        }
    }
    public void OnBuffAroundTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i =0; i< towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            // 이미 버프를 받고 있고, 현재 버프 타워의 레벨보다 높은 버프이면 패스
            if(weapon.BuffLevel > Level)
            {
                continue;
            }
            if(Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                if(weapon.WeaponType == WeaponType.Cannon || weapon.WeaponType == WeaponType.Laser || weapon.WeaponType == WeaponType.RangeAttack)
                {
                    weapon.BuffRate = towerTemplate.weapon[level].buff;

                    weapon.BuffLevel = Level;
                }
            }
        }
    }
    private bool IsPossibleToAttackTarget()
    {
        // target이 있는지 검사
        if(attackTarget == null)
        {
            return false;
        }
        // target이 공격 범위 안에 있는지 검사
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }
    private void SpawnProjectile()
    {
        float rate = towerTemplate.weapon[level].rate - BuffRate;
        if (weaponType == WeaponType.RangeAttack)
        {
            GameObject clone = Instantiate(projectileRangePrefab, spawnPoint.position, Quaternion.identity);
            clone.GetComponent<ProjectileRange>().Setup(attackTarget, towerTemplate.weapon[level].damage, rate, Radius);
        }
        else {
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            // 생성된 발사체에게 공격대상(attackTarget) 정보 제공
            clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage, rate);
        }
    }
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }
    private Transform currentTarget;
    private float accumulatedDamage = 0;
    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        float initialDamage = towerTemplate.weapon[level].damage;

        // 같은 방향으로 여러 개의 광선을 쏴서 그 중 현재 attackTarget과 동일한 오브젝트를 검출
        foreach (RaycastHit2D hit in hits)
        {
            // 선의 시작지점
            lineRenderer.SetPosition(0, spawnPoint.position);
            // 선의 목표지점
            lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, 0) + Vector3.back);
            // 타격 효과 위치 설정
            hitEffect.position = hit.point;


            // 적 체력 감소
            if (attackTarget != null && attackTarget == hit.collider.transform)
            {
                if (currentTarget == attackTarget)
                {
                    accumulatedDamage += damageIncreaseRate * Time.deltaTime;
                }
                else
                {
                    currentTarget = attackTarget;
                    accumulatedDamage = 0;
                }

                float damageToApply = initialDamage + accumulatedDamage;
                attackTarget.GetComponent<EnemyHP>().TakeDamage(damageToApply * Time.deltaTime);
            }
            else
            {
                currentTarget = hit.collider.transform;
                accumulatedDamage = 0;

                float damageToApply = initialDamage + damageIncreaseRate * Time.deltaTime;
                currentTarget.GetComponent<EnemyHP>().TakeDamage(damageToApply * Time.deltaTime);
            }
        }
    }

    public bool Upgrade()
    {
        if(playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }

        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;
        if(weaponType == WeaponType.Laser)
        {
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        // 타워가 업그레이드 될 때 모든 버프 타워의 버프 효과 갱신
        // 현재 타워가 버프 타워인 경우, 현재 타워가 공격 타워인 경우
        towerSpawner.OnBuffAllBuffTowers();

        return true;
    }
    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        ownerTile.IsBuildTower = false;
        Destroy(gameObject);
    }

    public void MergeUpgrade()
    {
        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;

        //애니메이션 바꾸기
        //_Animator.runtimeAnimatorController = towerTemplate.weapon[level].animatorController;

        //등급에 따른 애니메이션 트리거 세팅
        switch (level)
        {
            case 1:
                _Animator.SetBool("Level1", false);
                _Animator.SetBool("Level2", true);
                _Animator.SetBool("Level3", false);
                break;

            case 2:
                _Animator.SetBool("Level1", false);
                _Animator.SetBool("Level2", false);
                _Animator.SetBool("Level3", true);
                break;

            default:
                break;
        }

        if (weaponType == WeaponType.Laser)
        {
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        // 타워가 업그레이드 될 때 모든 버프 타워의 버프 효과 갱신
        // 현재 타워가 버프 타워인 경우, 현재 타워가 공격 타워인 경우
        towerSpawner.OnBuffAllBuffTowers();
    }

    public void MergeSell()
    {
        ownerTile.IsBuildTower = false;
        Destroy(gameObject);
    }
}
