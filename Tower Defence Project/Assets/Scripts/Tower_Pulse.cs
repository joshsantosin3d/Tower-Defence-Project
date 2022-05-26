using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Pulse : Tower
{
    public GameObject effectprefab;
    protected override void DamageTarget()
    {
        Collider[] nearbyColliders;     //Store a temp list of all nearby colliders
        List<Creep> nearbyCreeps = new List<Creep>();           //Store a temp list of all nearby creeps

        //Find all colliders within range.

        nearbyColliders = Physics.OverlapSphere(transform.position, range);

        bool nearbyEnemy = false;

        //Filter through the array and make a list of creeps.
        for (int i = 0; i < nearbyColliders.Length; i++)
        {
            Creep tempCreep = nearbyColliders[i].GetComponent<Creep>();

            //Avoid the null reference exception error when we inevitably check the ground and there's no creep component on it.
            if (tempCreep != null)
            {
                //nearbyCreeps.Add(tempCreep);
                tempCreep.TakeDamage(damage, this);
                nearbyEnemy = true;
            }
        }

        if (nearbyEnemy)
        {
            GameObject newEffect = Instantiate(effectprefab, transform.position, transform.rotation);

            LeanTween.scale(newEffect, new Vector3(range*2, 1, range*2), 0.2f);

            LeanTween.color(newEffect, Color.clear, 0.4f);

            Destroy(newEffect, 1);
        }

    }
}
