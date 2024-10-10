using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    public float damage = 1f;

    private float currentLifetime;

    private void OnEnable()
    {
        currentLifetime = 0f;
        
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

        currentLifetime += Time.deltaTime;
        if (currentLifetime >= lifetime)
        {
            gameObject.SetActive(false);
        }
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
            gameObject.SetActive(false);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}