using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    private const float TIME_TO_DESTROY = 10F;

    [SerializeField] private int scoreAdd = 10;
    [SerializeField] private int maxHP = 1;

    private readonly string setActiveFalse = "SetActiveFalse";

    private int currentHP = 0;

    public int ScoreAdd { get => scoreAdd; private set => scoreAdd = value; }

    public static event Action<int> OnDamage = null;
    public static event Action<Target> OnEnemyDie = null;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void OnEnable()
    {
        Invoke(setActiveFalse, TIME_TO_DESTROY);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collidedObjectLayer = collision.gameObject.layer;

        if (collidedObjectLayer.Equals(Utils.BulletLayer))
        {
            currentHP -= 1;

            if (currentHP <= 0)
            {
                OnEnemyDie?.Invoke(this);
                SetActiveFalse();
            }
        }
        else if (collidedObjectLayer.Equals(Utils.PlayerLayer) || collidedObjectLayer.Equals(Utils.KillVolumeLayer))
        {
            OnDamage?.Invoke(1);
            SetActiveFalse();
        }
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}