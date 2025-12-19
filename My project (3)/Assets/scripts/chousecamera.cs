using UnityEngine;
using System.Collections;
using LogitechG29.Sample.Input;
using UnityEngine.SceneManagement;

public class CarSelectionOrbit : MonoBehaviour
{
    [Header("Машины для выбора")]
    public Transform car1;
    public Transform car2;
    [SerializeField] InputControllerReader reader;

    [Header("Настройки камеры")]
    public Camera selectionCamera;
    public float transitionDuration = 1.5f;
    public float orbitDistance = 6f;
    public float orbitHeight = 1.5f;
    public float orbitSpeed = 10f;

    [Header("Текущее состояние")]
    public int currentCarIndex = 0;
    public bool isOrbiting = true;

    private bool isTransitioning = false;
    private float orbitAngle = 0f;
    private Transform currentTarget;

    void Start()
    {
        if (selectionCamera == null)
            selectionCamera = Camera.main;

        currentTarget = car1;
    }

    void Update()
    {
        HandleInput();

        if (!isTransitioning)
        {
            if (isOrbiting)
            {
                OrbitAroundCar();
            }
            else
            {
                LookAtCar();
            }
        }
    }

    void HandleInput()
    {
        if (isTransitioning) return;

        if (reader.Plus || reader.Minus)
        {
            SwitchToNextCar();
        }

        

        // Включение/выключение орбиты
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOrbiting = !isOrbiting;
        }
    }

    void SwitchToNextCar()
    {
        int nextIndex = (currentCarIndex + 1) % 2;
        StartCoroutine(SwitchCarCoroutine(nextIndex));
    }

    IEnumerator SwitchCarCoroutine(int newIndex)
    {
        isTransitioning = true;

        Transform startTarget = currentTarget;
        Transform endTarget = (newIndex == 0) ? car1 : car2;

        Vector3 startPos = selectionCamera.transform.position;
        Quaternion startRot = selectionCamera.transform.rotation;

        // Рассчитываем конечную позицию
        Vector3 endPos = CalculateOrbitPosition(endTarget, 0f);
        Quaternion endRot = Quaternion.LookRotation(
            endTarget.position + Vector3.up * orbitHeight - endPos,
            Vector3.up
        );

        // Эффект отъезда и подъезда
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime / transitionDuration;

            // Кривая для плавного ускорения и замедления
            float curveProgress = Mathf.SmoothStep(0f, 1f, progress);

            if (progress < 0.5f)
            {
                // Первая половина: отъезд от текущей машины
                float subProgress = curveProgress * 2f;
                Vector3 intermediatePos = Vector3.Lerp(
                    startPos,
                    startPos + Vector3.up * 3f,
                    subProgress
                );

                selectionCamera.transform.position = intermediatePos;
                selectionCamera.transform.rotation = Quaternion.Slerp(
                    startRot,
                    Quaternion.LookRotation(Vector3.down, Vector3.forward),
                    subProgress
                );
            }
            else
            {
                // Вторая половина: подъезд к новой машине
                float subProgress = (curveProgress - 0.5f) * 2f;
                Vector3 intermediatePos = Vector3.Lerp(
                    endPos + Vector3.up * 3f,
                    endPos,
                    subProgress
                );

                selectionCamera.transform.position = intermediatePos;
                selectionCamera.transform.rotation = Quaternion.Slerp(
                    Quaternion.LookRotation(Vector3.down, Vector3.forward),
                    endRot,
                    subProgress
                );
            }

            yield return null;
        }

        // Устанавливаем окончательные значения
        currentTarget = endTarget;
        currentCarIndex = newIndex;
        isTransitioning = false;

        PlayerPrefs.SetInt("SelectedCar", currentCarIndex);

        Debug.Log($"Переключено на машину {currentCarIndex}");
    }

    void OrbitAroundCar()
    {
        if (currentTarget == null) return;

        orbitAngle += Time.deltaTime * orbitSpeed;

        Vector3 orbitPos = CalculateOrbitPosition(currentTarget, orbitAngle);
        Quaternion lookRotation = Quaternion.LookRotation(
            currentTarget.position + Vector3.up * orbitHeight - orbitPos,
            Vector3.up
        );

        selectionCamera.transform.position = orbitPos;
        selectionCamera.transform.rotation = lookRotation;
    }

    void LookAtCar()
    {
        if (currentTarget == null) return;

        Vector3 targetPos = currentTarget.position +
                           currentTarget.forward * -orbitDistance +
                           Vector3.up * orbitHeight;

        selectionCamera.transform.position = Vector3.Lerp(
            selectionCamera.transform.position,
            targetPos,
            Time.deltaTime * 3f
        );

        Quaternion targetRot = Quaternion.LookRotation(
            currentTarget.position + Vector3.up * 1f - selectionCamera.transform.position,
            Vector3.up
        );

        selectionCamera.transform.rotation = Quaternion.Slerp(
            selectionCamera.transform.rotation,
            targetRot,
            Time.deltaTime * 3f
        );
    }

    Vector3 CalculateOrbitPosition(Transform target, float angle)
    {
        float x = Mathf.Sin(angle) * orbitDistance;
        float z = Mathf.Cos(angle) * orbitDistance;

        return target.position + new Vector3(x, orbitHeight, z);
    }

    
    public void SelectCar(int index)
    {
        if (!isTransitioning && currentCarIndex != index)
        {
            StartCoroutine(SwitchCarCoroutine(index));
        }
    }
}