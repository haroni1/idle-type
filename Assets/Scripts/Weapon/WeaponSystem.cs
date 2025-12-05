using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [System.Serializable]
    public class WeaponInstance
    {
        public WeaponData data;
        public int level = 1;
        [HideInInspector] public float cooldownTimer = 0f;
    }

    public List<WeaponInstance> weapons = new List<WeaponInstance>();

    [Header("공격 대상 설정")]
    public LayerMask enemyLayer;      // Chicken 레이어 지정
    public float targetSearchRadius = 10f;

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.currentState != GameState.Day) return; // 낮일 때만 공격

        foreach (var weapon in weapons)
        {
            if (weapon.data == null) continue;

            weapon.cooldownTimer -= Time.deltaTime;
            if (weapon.cooldownTimer <= 0f)
            {
                TryFire(weapon);
            }
        }
    }

    private void TryFire(WeaponInstance weapon)
    {
        Transform target = FindNearestEnemy(weapon.data.range);
        if (target == null)
        {
            // 공격할 적이 없으면 쿨타임 리셋만
            weapon.cooldownTimer = GetCurrentCooldown(weapon);
            return;
        }

        int projectileCount = GetCurrentProjectileCount(weapon);
        float damage = GetCurrentDamage(weapon);
        float speed = GetCurrentSpeed(weapon);

        // 여러 발 발사 (부채꼴, 회전 등은 나중에)
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projObj = Instantiate(weapon.data.projectilePrefab, transform.position, Quaternion.identity);
            Projectile proj = projObj.GetComponent<Projectile>();
            proj.Init(target, speed, Mathf.RoundToInt(damage));
        }

        weapon.cooldownTimer = GetCurrentCooldown(weapon);
    }

    private Transform FindNearestEnemy(float range)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    #region 현재 스탯 계산

    private float GetCurrentDamage(WeaponInstance weapon)
    {
        return weapon.data.baseDamage + weapon.data.damagePerLevel * (weapon.level - 1);
    }

    private float GetCurrentCooldown(WeaponInstance weapon)
    {
        float cd = weapon.data.baseCooldown * Mathf.Pow(weapon.data.cooldownMultiplierPerLevel, weapon.level - 1);
        return Mathf.Max(0.1f, cd);
    }

    private int GetCurrentProjectileCount(WeaponInstance weapon)
    {
        return weapon.data.baseProjectileCount + weapon.data.projectileCountPerLevel * (weapon.level - 1);
    }

    private float GetCurrentSpeed(WeaponInstance weapon)
    {
        return weapon.data.baseSpeed; // 필요하면 레벨당 조정
    }

    #endregion

    // 상점/카드에서 호출할 함수들

    public void AddWeapon(WeaponData data)
    {
        // 이미 있으면 레벨업, 없으면 새로 추가
        var existing = weapons.Find(w => w.data == data);
        if (existing != null)
        {
            existing.level++;
        }
        else
        {
            WeaponInstance newWeapon = new WeaponInstance
            {
                data = data,
                level = 1,
                cooldownTimer = 0f
            };
            weapons.Add(newWeapon);
        }

        Debug.Log($"무기 획득/레벨업: {data.weaponName}");
    }

    public void UpgradeWeapon(WeaponData data, int levelIncrease = 1)
    {
        var existing = weapons.Find(w => w.data == data);
        if (existing != null)
        {
            existing.level += levelIncrease;
            Debug.Log($"무기 레벨업: {data.weaponName} Lv.{existing.level}");
        }
    }
}
