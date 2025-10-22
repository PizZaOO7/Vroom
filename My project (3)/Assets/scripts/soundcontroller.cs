using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    public AudioSource engineAudioSource;

    // ����������� � ������������ ���������
    public float minVolume = 0.1f;
    public float maxVolume = 0.5f;

    // ����������� � ������������ ������ ���� (Pitch)
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;

    // ������ �� ������, ����������� ������� (�������� �� ����)
    public CarControllerSample carController;

    // ������ �� Rigidbody ������ (���� ���������� ����� ������)
    public Rigidbody carRigidbody;

    // ������������ �������� ������ ��� ��������
    public float maxSpeed = 50f;

    void Start()
    {
        // ������������� ������� ����������, ���� ��� �� ��������� �������
        if (engineAudioSource == null)
            engineAudioSource = GetComponent<AudioSource>();

        if (carRigidbody == null)
            carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ���� ��� ������ �����������, ������� �� ������
        if (carRigidbody == null) return;

        // �������� ������� �������� ������.
        // ������� 1: ���� � ������ ������� ���������� ���� ���������� speed.
        // float currentSpeed = carController.speed;

        // ������� 2: ����� Rigidbody (������, ���� ���������� �� ������).
        float currentSpeed = carRigidbody.linearVelocity.magnitude;

        // ������������ ��������������� �������� (�� 0 �� 1)
        float normalizedSpeed = currentSpeed / maxSpeed;
        // ������������ �������� �� 0 �� 1
        normalizedSpeed = Mathf.Clamp01(normalizedSpeed);

        // ������ ��������� � ����������� �� ��������
        engineAudioSource.volume = Mathf.Lerp(minVolume, maxVolume, normalizedSpeed);

        // ������ ������ ���� (���� ���������� "����" �� ��������)
        engineAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
    }
}