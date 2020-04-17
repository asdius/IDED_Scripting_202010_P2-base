using System;
using UnityEngine;
using Pools;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    public const int PLAYER_LIVES = 3;

    private const float PLAYER_RADIUS = 0.4F;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 1F;

    private float hVal;

    #region Bullet
    [SerializeField] private Transform bulletSpawnPoint = null;
    private Bullet m_Bullet = null;
    #endregion Bullet

    #region BoundsReferences

    private float referencePointComponent;
    private float leftCameraBound;
    private float rightCameraBound;

    #endregion BoundsReferences

    #region StatsProperties

    public int Score { get; set; }
    public int Lives { get; set; }

    #endregion StatsProperties

    #region MovementProperties

    private readonly string horizontalAxis = "Horizontal";

    public bool ShouldMove
    {
        get =>
            (hVal != 0F && InsideCamera) ||
            (hVal > 0F && ReachedLeftBound) ||
            (hVal < 0F && ReachedRightBound);
    }

    private bool InsideCamera
    {
        get => !ReachedRightBound && !ReachedLeftBound;
    }

    private bool ReachedRightBound { get => referencePointComponent >= rightCameraBound; }
    private bool ReachedLeftBound { get => referencePointComponent <= leftCameraBound; }

    private bool CanShoot { get => bulletSpawnPoint != null; }

    #endregion MovementProperties

    public static event Action OnPlayerHit = null;
    public static event Action<int> OnPlayerScoreChanged = null;
    public static event Action OnPlayerDied = null;

    private void Awake()
    {
        leftCameraBound = Camera.main.ViewportToWorldPoint(new Vector3(0F, 0F, 0F)).x + PLAYER_RADIUS;
        rightCameraBound = Camera.main.ViewportToWorldPoint(new Vector3(1F, 0F, 0F)).x - PLAYER_RADIUS;

        Lives = PLAYER_LIVES;
    }

    private void Start()
    {
        Target.OnDamage += TakeDamage;
        Target.OnEnemyDie += UpdatePoints;
    }

    private void Update()
    {
        hVal = Input.GetAxis(horizontalAxis);

        if (ShouldMove)
        {
            transform.Translate(transform.right * hVal * moveSpeed * Time.deltaTime);
            referencePointComponent = transform.position.x;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && CanShoot)
        {
            m_Bullet = BulletPool.Instance.GetBullet().GetComponent<Bullet>();
            m_Bullet.transform.position = bulletSpawnPoint.position;
            m_Bullet.transform.rotation = Quaternion.identity;
            m_Bullet.Rigidbody.AddForce(transform.up * m_Bullet.Speed, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Take damage from enemy
    /// </summary>
    /// <param name="_damage"></param>
    private void TakeDamage(int _damage)
    {
        Debug.Log("Campi, me hacen daño.");
        Lives -= _damage;
        OnPlayerHit?.Invoke();

        if (Lives <= 0)
        {
            enabled = false;
            gameObject.SetActive(false);
            OnPlayerDied?.Invoke();
        }
    }

    /// <summary>
    /// Add points
    /// </summary>
    /// <param name="_enemyDestroyed"></param>
    private void UpdatePoints(Target _enemyDestroyed)
    {
        Score += _enemyDestroyed.ScoreAdd;
        OnPlayerScoreChanged?.Invoke(Score);
    }

    private void OnDestroy()
    {
        Target.OnDamage -= TakeDamage;
        Target.OnEnemyDie -= UpdatePoints;
    }
}