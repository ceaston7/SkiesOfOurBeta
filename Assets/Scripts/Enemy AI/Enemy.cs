using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    List<HomingMissile> lockedOnMissiles = new List<HomingMissile>();

    public void MissileLocked(HomingMissile missile)
    {
        lockedOnMissiles.Add(missile);
    }

    public void Kill()
    {
        for (int i = 0; i < lockedOnMissiles.Count; i++)
        {
            if (lockedOnMissiles[i] != null)
            {
                lockedOnMissiles[i].closestEnemy = null;
            }
        }

        Destroy(gameObject);
    }
}
