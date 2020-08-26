using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public static void Create(Vector3 spawnPosition, Unit unit, int damageAmount)
    {
        Transform projectileTransform = Instantiate(GameAssets.instance.projectile, spawnPosition, Quaternion.identity);
        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        projectile.Setup(unit, damageAmount);
    }

    private Unit unit;
    private int damageAmount;
    private void Setup(Unit unit, int damageAmount)
    {
        this.unit = unit;
        this.damageAmount = damageAmount;
    }

    private void Update()
    {
        Vector3 targetPosition = unit.GetPosition();
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 20f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float angle = Utilities.GetAngleFromVectorFloat(moveDir);
        transform.eulerAngles = new Vector3(0, 0, angle);

        float selfDestructDistance = 1f;
        if (Vector3.Distance(transform.position, targetPosition) < selfDestructDistance)
        {
            unit.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }


}
