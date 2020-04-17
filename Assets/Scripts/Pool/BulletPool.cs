using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class BulletPool : Pool<BulletPool>
    {
        [SerializeField] private Bullet gameObjectPrefab = null;

        private List<Bullet> bullets = new List<Bullet>();

        protected override void OnAwake()
        {
            InitializePool();
        }

        protected override void InitializePool()
        {
            bullets.Clear();

            for (int i = 0; i < initialInstances; i++)
            {
                Bullet gOClone = Instantiate(gameObjectPrefab, Vector3.zero, Quaternion.identity);
                gOClone.gameObject.SetActive(false);
                bullets.Add(gOClone);
            }
        }

        public Bullet GetBullet()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].gameObject.activeInHierarchy)
                {
                    bullets[i].gameObject.SetActive(true);
                    return bullets[i];
                }
            }

            Bullet gOClone = Instantiate(gameObjectPrefab, Vector3.zero, Quaternion.identity);
            gOClone.gameObject.SetActive(false);
            bullets.Add(gOClone);
            return gOClone;
        }
    } 
}
