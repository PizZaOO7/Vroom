using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LapTimer : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text currentLapTimeText;
    public TMP_Text bestLapTimeText;
    public TMP_Text lapCountText;

    [Header("Settings")]
    public int totalLaps = 3;

    private float currentLapTime = 0f;
    private float bestLapTime = Mathf.Infinity;
    private int currentLap = 0;
    private bool isRacing = false;

    // Событие для уведомления о завершении круга
    public System.Action<int, float> OnLapCompleted;

    void Start()
    {
        StartNewRace();
    }

    void Update()
    {
        if (isRacing)
        {
            currentLapTime += Time.deltaTime;
            UpdateUIText();
        }
    }

    public void StartNewRace()
    {
        currentLap = 0;
        currentLapTime = 0f;
        bestLapTime = Mathf.Infinity;
        isRacing = true;
        StartNewLap();
    }

    public void StartNewLap()
    {
        currentLap++;
        currentLapTime = 0f;

        if (currentLap > totalLaps)
        {
            FinishRace();
            return;
        }

        UpdateUIText();
        Debug.Log($"Начат круг {currentLap}");
    }

    public void CompleteLap()
    {
        if (!isRacing) return;

        // Проверяем, установлен ли новый рекорд
        if (currentLapTime < bestLapTime)
        {
            bestLapTime = currentLapTime;
        }

        // Вызываем событие завершения круга
        OnLapCompleted?.Invoke(currentLap, currentLapTime);

        Debug.Log($"Круг {currentLap} завершен за {FormatTime(currentLapTime)}");

        // Начинаем следующий круг
        StartNewLap();
    }

    private void FinishRace()
    {
        isRacing = false;
        Debug.Log("Гонка завершена!");
        // Здесь можно добавить логику завершения гонки
    }

    private void UpdateUIText()
    {
        if (currentLapTimeText != null)
            currentLapTimeText.text = $"Текущий круг: {FormatTime(currentLapTime)}";

        if (bestLapTimeText != null)
            bestLapTimeText.text = bestLapTime < Mathf.Infinity ?
                $"Лучший круг: {FormatTime(bestLapTime)}" : "Лучший круг: --:--.---";

        if (lapCountText != null)
            lapCountText.text = $"Круг: {currentLap}/{totalLaps}";
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);

        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    // Методы для получения текущих значений
    public float GetCurrentLapTime() => currentLapTime;
    public float GetBestLapTime() => bestLapTime;
    public int GetCurrentLap() => currentLap;
    public bool IsRacing() => isRacing;
}