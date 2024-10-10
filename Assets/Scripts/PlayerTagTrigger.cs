using UnityEngine;

public class PlayerTagTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Business") || other.CompareTag("Manager"))
        {
            // Lógica de activación para interactuar con los negocios o managers.
        }
    }
}
