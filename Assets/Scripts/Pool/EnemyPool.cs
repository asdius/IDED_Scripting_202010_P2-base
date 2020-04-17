using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class EnemyPool : Pool<EnemyPool>
    {
        [SerializeField] private GameObject[] enemies = new GameObject[3];

        private List<GameObject> enemyPool = new List<GameObject>();

        protected override void OnAwake()
        {
            InitializePool();
        }

        protected override void InitializePool()
        {
            enemyPool.Clear();

            int step = initialInstances / enemies.Length;
            GameObject gOClone = null;

            for (int i = 0; i < initialInstances; i++)
            {
                if (i < step)
                {
                    gOClone = Instantiate(enemies[0], Vector3.zero, Quaternion.identity);
                }
                else if (i >= step && i < step * 2)
                {
                    gOClone = Instantiate(enemies[1], Vector3.zero, Quaternion.identity);
                }
                else
                {
                    gOClone = Instantiate(enemies[2], Vector3.zero, Quaternion.identity);
                }

                gOClone.SetActive(false);
                enemyPool.Add(gOClone);
            }
        }

        public GameObject GetRandomEnemy()
        {
            int randomPosition = Random.Range(0, enemyPool.Count);
            int attempts = 5;

            GameObject result = null;

            do
            {
                result = enemyPool[randomPosition];

                if (!result.activeInHierarchy)
                {
                    return result;
                }
                attempts--;
            }
            while (attempts >= 0);

            GameObject gOClone = Instantiate(enemies[Random.Range(0, enemies.Length)]);
            gOClone.SetActive(false);
            enemyPool.Add(gOClone);

            return gOClone;
        }
    }
}
