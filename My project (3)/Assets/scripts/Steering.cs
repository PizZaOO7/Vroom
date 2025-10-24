using UnityEngine;
using LogitechG29.Sample.Input;

public class Steering : MonoBehaviour
{
    [SerializeField] InputControllerReader inputControllerReader;
    [SerializeField] float maxSteeringAngle = 900f;
    [SerializeField] float steeringSpeed = 5f;
    float currentSteeringAngle = 0f;
    float targetSteeringAngle = 0f;
    void Update()
    {
        float steeringInput = inputControllerReader.Steering;
        targetSteeringAngle = steeringInput * maxSteeringAngle;
        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, targetSteeringAngle, steeringSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentSteeringAngle);
    }
}