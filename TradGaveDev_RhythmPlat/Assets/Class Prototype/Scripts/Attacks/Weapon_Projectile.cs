using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Projectile : Weapon {

    public GameObject projectilePrefab;

    public float fireRate = 0.25f;

    private bool _canFire = true;

    public override void Fire (Transform attackSpawnPoint)
    {
        if (!_canFire) return;

        GameObject projectile = (GameObject)Instantiate(projectilePrefab, attackSpawnPoint.position, attackSpawnPoint.rotation, null);

        _canFire = false;

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown ()
    {
        yield return new WaitForSeconds(fireRate);

        _canFire = true;
    }
}
