using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera;    // Cámara a la que el sprite debe orientarse
    public Vector3 axisToAlign = Vector3.forward;  // Eje del sprite que se alineará hacia la cámara (puedes definirlo manualmente)
    public bool alignToNormal = false;  // Si se orienta en base a la normal del sprite o no

    void Start()
    {
        // Si no se asigna una cámara manualmente, usamos la cámara principal por defecto
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            // Calculamos la dirección hacia la cámara
            Vector3 directionToCamera = targetCamera.transform.position - transform.position;

            // Si queremos que el sprite se oriente según su normal predeterminada
            if (alignToNormal)
            {
                transform.forward = -directionToCamera.normalized;  // Orientamos el frente del sprite hacia la cámara
            }
            else
            {
                // Usamos el eje definido por el usuario para orientar el sprite
                directionToCamera.y = 0;  // Evitamos inclinaciones hacia arriba o abajo (puedes ajustar esto según tu caso)
                Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera.normalized);

                // Rotamos solo el eje que el usuario especificó
                transform.rotation = Quaternion.LookRotation(targetRotation * axisToAlign);
            }
        }
    }
}
