using UnityEngine;
using System.Collections.Generic;

using LogitechG29.Sample.Input;



public class SimpleTireWearController : MonoBehaviour
{
    [Header("Настройки шин")]
    public List<SimpleTireWear> tires = new List<SimpleTireWear>();

    [Header("Настройки износа")]
    public float wearMultiplier = 1f; // Множитель скорости износа

    [Header("UI (опционально)")]
    public UnityEngine.UI.Text wearInfoText;

    private float updateInterval = 0.1f;
    private float timer = 0f;
    private Dictionary<WheelCollider, Vector3> lastWheelPositions = new Dictionary<WheelCollider, Vector3>();
    [SerializeField]InputControllerReader reader;

    
    public AudioSource pitsource;
    void Start()
    {
        
        foreach (var tire in tires)
        {
            if (tire.wheelCollider != null)
            {
                tire.Initialize();
                lastWheelPositions[tire.wheelCollider] = tire.wheelCollider.transform.position;
            }
        }

        UpdateUI();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pitstop"))
        {
            Debug.Log("pitstop");
            RepairAllTires();
            
            pitsource.Play();
        }
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateAllTires();
            UpdateUI();
        }

        // Тестовая кнопка
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestTireWear();
        }

        
    }

    void UpdateAllTires()
    {
        foreach (var tire in tires)
        {
            if (tire.wheelCollider == null) continue;

            Vector3 currentPos = tire.wheelCollider.transform.position;

            // Получаем последнюю позицию этого колеса
            if (lastWheelPositions.ContainsKey(tire.wheelCollider))
            {
                Vector3 lastPos = lastWheelPositions[tire.wheelCollider];
                float distance = Vector3.Distance(lastPos, currentPos);

                // Проверяем, действительно ли колесо двигалось
                if (distance > 0.001f && IsWheelGrounded(tire.wheelCollider))
                {
                    // Применяем износ с учетом множителя
                    tire.UpdateWear(distance * wearMultiplier * 10f); // Умножаем на 10 для более заметного эффекта
                }

                // Обновляем позицию
                lastWheelPositions[tire.wheelCollider] = currentPos;
            }
            else
            {
                // Первая инициализация
                lastWheelPositions[tire.wheelCollider] = currentPos;
            }
        }
    }

    bool IsWheelGrounded(WheelCollider wheel)
    {
        WheelHit hit;
        return wheel.GetGroundHit(out hit);
    }

    float CalculateWheelSlip(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            // Сумма продольного и бокового скольжения
            float totalSlip = Mathf.Abs(hit.forwardSlip) + Mathf.Abs(hit.sidewaysSlip);
            return totalSlip;
        }
        return 0f;
    }

    void UpdateUI()
    {
        if (wearInfoText != null)
        {
            float avgWear = GetAverageWear();
            wearInfoText.text = $"Износ шин: {avgWear:F1}%\n" +
                              $"Сцепление: {GetTractionMultiplier():F2}\n";
                              
        }
    }

    public float GetAverageWear()
    {
        if (tires.Count == 0) return 100f;

        float total = 0;
        int count = 0;
        foreach (var tire in tires)
        {
            if (tire.wheelCollider != null)
            {
                total += tire.GetWearPercentage();
                count++;
            }
        }
        return count > 0 ? total / count : 100f;
    }

    public float GetTractionMultiplier()
    {
        // Возвращает множитель сцепления (от 0.3 до 1.0)
        return Mathf.Lerp(0.3f, 1f, GetAverageWear() / 100f);
    }

    public float GetTotalDistance()
    {
        float total = 0;
        foreach (var tire in tires)
        {
            total += tire.distanceTraveled;
        }
        return tires.Count > 0 ? total / tires.Count : 0f;
    }

    public void RepairAllTires()
    {
        foreach (var tire in tires)
        {
            tire.ResetWear();
        }
        Debug.Log("Все шины отремонтированы!");
        UpdateUI();
    }

    // Тестовая функция
    void TestTireWear()
    {
        Debug.Log($"=== Тест износа шин ===");
        Debug.Log($"Средний износ: {GetAverageWear():F1}%");
        Debug.Log($"Множитель сцепления: {GetTractionMultiplier():F2}");
        Debug.Log($"Общий пробег: {GetTotalDistance():F0} м");

        // Быстрый износ для теста
        foreach (var tire in tires)
        {
            if (tire.wheelCollider != null)
            {
                // Износ на 20% для теста
                tire.UpdateWear(200f); // Эквивалент 200 метров движения
            }
        }

        Debug.Log($"После теста: {GetAverageWear():F1}%");
    }

    // Для дебага
    void OnGUI()
    {
        if (tires.Count > 0)
        {
            GUI.Box(new Rect(10, 10, 250, 130), "Износ шин");
            GUI.Label(new Rect(20, 40, 230, 20), $"Средний износ: {GetAverageWear():F1}%");
            GUI.Label(new Rect(20, 60, 230, 20), $"Сцепление: {GetTractionMultiplier():F2}");
            

            
        }
    }
}