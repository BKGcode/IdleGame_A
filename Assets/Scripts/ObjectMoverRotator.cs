using UnityEngine;
using DG.Tweening;

public class ObjectMoverRotator : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float rotationSpeed = 90f;

    private void Start()
    {
        // Iniciar el movimiento de arriba a abajo
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // Iniciar la rotación continua
        transform.DORotate(new Vector3(0, 360, 0), 360f / rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    private void OnDestroy()
    {
        // Asegurarse de que todos los tweens asociados a este objeto se detengan
        DOTween.Kill(transform);
    }

    // Métodos públicos para ajustar las velocidades en tiempo de ejecución
    public void SetMovementSpeed(float newDuration)
    {
        moveDuration = newDuration;
        DOTween.Kill(transform, false);
        Start();
    }

    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
        DOTween.Kill(transform, false);
        Start();
    }
}