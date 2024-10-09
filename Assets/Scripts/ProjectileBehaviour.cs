using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    public float damage = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        
        // Asegurarse de que el collider esté configurado como trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyPatrolController enemy = other.GetComponent<EnemyPatrolController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}