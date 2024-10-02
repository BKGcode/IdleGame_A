using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera;    // C�mara a la que el sprite debe orientarse
    public Vector3 axisToAlign = Vector3.forward;  // Eje del sprite que se alinear� hacia la c�mara (puedes definirlo manualmente)
    public bool alignToNormal = false;  // Si se orienta en base a la normal del sprite o no

    void Start()
    {
        // Si no se asigna una c�mara manualmente, usamos la c�mara principal por defecto
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            // Calculamos la direcci�n hacia la c�mara
            Vector3 directionToCamera = targetCamera.transform.position - transform.position;

            // Si queremos que el sprite se oriente seg�n su normal predeterminada
            if (alignToNormal)
            {
                transform.forward = -directionToCamera.normalized;  // Orientamos el frente del sprite hacia la c�mara
            }
            else
            {
                // Usamos el eje definido por el usuario para orientar el sprite
                directionToCamera.y = 0;  // Evitamos inclinaciones hacia arriba o abajo (puedes ajustar esto seg�n tu caso)
                Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera.normalized);

                // Rotamos solo el eje que el usuario especific�
                transform.rotation = Quaternion.LookRotation(targetRotation * axisToAlign);
            }
        }
    }
}
