using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageAdjuster : MonoBehaviour
{
    [MMInformation("This script gets the MMObjectPool of the assigned BulletPool and adjusts the damage of each bullet in the pool. The MMObjectPooler should not be expandable since this script ideally only runs once in the beginning of the game to adjust the damage.", MMInformationAttribute.InformationType.None, false)]
    public MMSimpleObjectPooler BulletPool;

    public void AdjustBulletPoolDamage(float multiplier)
    {
        MMObjectPool pool = BulletPool.ExistingPool("[SimpleObjectPooler] " + BulletPool.GameObjectToPool.name);

        if (pool != null )
        {
            foreach (var item in pool.PooledGameObjects)
            {
                BulletAdjuster bullet = item.GetComponent<BulletAdjuster>();
                if (bullet != null )
                {
                    bullet.MultiplyDamage(multiplier);
                }
            }

        }
    }
}
