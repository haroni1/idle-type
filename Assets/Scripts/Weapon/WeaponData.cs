using UnityEngine;

public enum WeaponType
{
    Basic,
    Shotgun,
    Laser,
    // 나중에 계속 추가
}

[CreateAssetMenu(menuName = "ChickenGame/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType type;

    [Header("투사체 설정")]
    public GameObject projectilePrefab;
    public float baseDamage = 1f;
    public float baseSpeed = 10f;
    public float baseCooldown = 1f;
    public int baseProjectileCount = 1;
    public float range = 8f;

    [Header("레벨 업 계수 (옵션)")]
    public float damagePerLevel = 0.5f;
    public float cooldownMultiplierPerLevel = 0.9f;
    public int projectileCountPerLevel = 0;
}
