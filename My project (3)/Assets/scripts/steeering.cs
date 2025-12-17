using UnityEngine;
using LogitechG29.Sample.Input;

public class Steeering : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    [SerializeField] float maxSteeringAngle = 900f;
    [SerializeField] float steeringSpeed = 5f;

    private float currentSteeringAngle = 0f;
    private float targetSteeringAngle = 0f;
    private Quaternion initialRotation; // Сохраняем начальный поворот

    void Start()
    {
        // Сохраняем начальный поворот объекта
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float steeringInput = inputControllerReader.Steering;
        targetSteeringAngle = steeringInput * maxSteeringAngle;

        currentSteeringAngle = Mathf.Lerp(
            currentSteeringAngle,
            targetSteeringAngle,
            steeringSpeed * Time.deltaTime
        );

        // Сохраняем начальный поворот и добавляем только поворот по Z
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, currentSteeringAngle);
    }
}