using UnityEngine;

[System.Serializable]
public class SimpleTireWear
{
    public WheelCollider wheelCollider;
    public float currentWear = 100f;
    public float distanceTraveled = 0f;
    private Vector3 lastPosition;
    private WheelFrictionCurve originalForwardFriction;
    private WheelFrictionCurve originalSidewaysFriction;

    public void Initialize()
    {
        if (wheelCollider == null) return;

        lastPosition = wheelCollider.transform.position;
        originalForwardFriction = wheelCollider.forwardFriction;
        originalSidewaysFriction = wheelCollider.sidewaysFriction;
    }

    public void UpdateWear(float distance)
    {
        if (wheelCollider == null) return;

        distanceTraveled += distance;

        // Износ: 1% на каждые 1000 метров
        float wearAmount = (distance / 1000f) * 1f;
        currentWear = Mathf.Max(0, currentWear - wearAmount);

        // Обновляем трение
        UpdateFriction();
    }

    private void UpdateFriction()
    {
        // Коэффициент трения от 30% до 100%
        float frictionMultiplier = Mathf.Lerp(0.3f, 1f, currentWear / 100f);

        WheelFrictionCurve newForward = originalForwardFriction;
        newForward.stiffness = originalForwardFriction.stiffness * frictionMultiplier;
        wheelCollider.forwardFriction = newForward;

        WheelFrictionCurve newSideways = originalSidewaysFriction;
        newSideways.stiffness = originalSidewaysFriction.stiffness * frictionMultiplier;
        wheelCollider.sidewaysFriction = newSideways;
    }

    public float GetWearPercentage()
    {
        return currentWear;
    }

    public void ResetWear()
    {
        currentWear = 100f;
        distanceTraveled = 0f;
        if (wheelCollider != null)
        {
            wheelCollider.forwardFriction = originalForwardFriction;
            wheelCollider.sidewaysFriction = originalSidewaysFriction;
        }
    }
}