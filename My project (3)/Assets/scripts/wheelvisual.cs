using UnityEngine;

public class WheelColliderVisuals : MonoBehaviour
{
    [System.Serializable]
    public class WheelData
    {
        public Transform visualWheel;
        public WheelCollider wheelCollider;
        public bool isSteeringWheel = false;
    }

    [Header("Wheel Setup")]
    public WheelData frontLeft;
    public WheelData frontRight;
    public WheelData rearLeft;
    public WheelData rearRight;

    [Header("Steering Settings")]
    public float maxSteeringAngle = 30f;

    private void Update()
    {
        UpdateWheelVisual(frontLeft);
        UpdateWheelVisual(frontRight);
        UpdateWheelVisual(rearLeft);
        UpdateWheelVisual(rearRight);
    }

    private void UpdateWheelVisual(WheelData wheelData)
    {
        if (wheelData.visualWheel == null || wheelData.wheelCollider == null) return;

        // Получаем позицию и вращение от WheelCollider
        wheelData.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        // Применяем к визуальному колесу
        wheelData.visualWheel.position = position;
        wheelData.visualWheel.rotation = rotation;
    }

    public void ApplySteering(float steeringInput)
    {
        float steeringAngle = steeringInput * maxSteeringAngle;

        if (frontLeft.wheelCollider != null)
            frontLeft.wheelCollider.steerAngle = steeringAngle;
        if (frontRight.wheelCollider != null)
            frontRight.wheelCollider.steerAngle = steeringAngle;
    }
}