using UnityEngine;

public class WheelColliderVisuals : MonoBehaviour
{
    [System.Serializable]
    public class WheelData
    {
        public Transform visualWheel1;
        public Transform visualWheel2;
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
        if (wheelData.visualWheel2 == null || wheelData.wheelCollider == null) return;

        // Получаем позицию и вращение от WheelCollider
        wheelData.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        // Применяем к визуальному колесу
        wheelData.visualWheel1.position = position;
        wheelData.visualWheel1.rotation = rotation;
        wheelData.visualWheel2.position = position;
        wheelData.visualWheel2.rotation = rotation;
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