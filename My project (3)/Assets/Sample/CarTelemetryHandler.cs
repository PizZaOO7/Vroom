using System;
using System.Collections;
using _2DOF;
using UnityEngine;

public class CarTelemetryHandler : MonoBehaviour
{
    private const float WAIT_TIME = SendingData.WAIT_TIME / 1000f;

    [SerializeField] private Transform vehicleTransform;
    [SerializeField] private Rigidbody rigidbody;


    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float accelerationSensitivity = 0.5f;

    private ObjectTelemetryData _telemetryDataData;
    private SendingData _sendingData;
    private Vector3 _lastVelocity;
    private Vector3 _currentAcceleration;

    private void Awake()
    {
        _sendingData = new SendingData();
        _telemetryDataData = _sendingData.ObjectTelemetryData;
        _lastVelocity = rigidbody.linearVelocity;
    }

    public void OnEnable()
    {
        StartCoroutine(TelemetryHandler());
        _sendingData.SendingStart();
    }

    public void OnDisable()
    {
        StopCoroutine(TelemetryHandler());
        _sendingData.SendingStop();
    }

    private IEnumerator TelemetryHandler()
    {
        while (true)
        {
            if (_telemetryDataData == null)
            {
                yield return new WaitForSeconds(WAIT_TIME * 10f);
                continue;
            }

            UpdateAngles();
            UpdateVelocity();
            CalculateAcceleration();

            yield return new WaitForSeconds(WAIT_TIME);
        }
    }

    private void CalculateAcceleration()
    {

        _currentAcceleration = (rigidbody.linearVelocity - _lastVelocity) / WAIT_TIME;
        _lastVelocity = rigidbody.linearVelocity;
    }

    private void UpdateVelocity()
    {
        _telemetryDataData.Velocity = rigidbody.linearVelocity;
    }

    private void UpdateAngles()
    {
        var euler = vehicleTransform.eulerAngles;

        euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
        euler.y = euler.y > 180 ? euler.y - 360 : euler.y;
        euler.z = euler.z > 180 ? euler.z - 360 : euler.z;


        float accelerationTilt = CalculateAccelerationTilt();
        euler.x += accelerationTilt;


        euler.x = Mathf.Clamp(euler.x, -maxTiltAngle, maxTiltAngle);
        euler.z = Mathf.Clamp(euler.z, -maxTiltAngle, maxTiltAngle);

        _telemetryDataData.Angles = euler;
    }

    private float CalculateAccelerationTilt()
    {

        Vector3 localAcceleration = vehicleTransform.InverseTransformDirection(_currentAcceleration);
        float forwardAcceleration = localAcceleration.z;


        float tiltAngle = forwardAcceleration * accelerationSensitivity;


        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);

        return tiltAngle;
    }
}
