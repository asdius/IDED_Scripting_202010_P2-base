using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private float DESTROY_TIME = 5f;

    private readonly string setActiveFalse = "SetActiveFalse";

    public Rigidbody Rigidbody { get; private set; } = null;
    public float Speed { get => speed; private set => speed = value; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke(setActiveFalse, DESTROY_TIME);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collidedObjectLayer = collision.gameObject.layer;

        if (collidedObjectLayer.Equals(Utils.EnemyLayer))
        {
            gameObject.SetActive(false);
        }
    }
}
