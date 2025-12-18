using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Lap : MonoBehaviour
{
    [Header("UI Elements")]
    public Text currentLapTimeText;
    public Text bestLapTimeText;
    public Text lapCounterText;

    [Header("Settings")]
    public int totalLaps = 3;

    private float currentLapTime = 0f;
    private float bestLapTime = Mathf.Infinity;
    private int currentLap = 1;
    private bool isRacing = false;

    void Start()
    {
        StartRace();
    }

    void Update()
    {
        if (isRacing)
        {
            currentLapTime += Time.deltaTime;
            UpdateUIText();
        }
    }

    public void StartRace()
    {
        isRacing = true;
        currentLap = 1;
        currentLapTime = 0f;
        UpdateUIText();
    }

    public void CompleteLap()
    {
        if (!isRacing) return;

        // Сохраняем лучшее время круга
        if (currentLapTime < bestLapTime)
        {
            bestLapTime = currentLapTime;
        }

        // Увеличиваем счетчик кругов
        currentLap++;

        // Сбрасываем таймер текущего круга
        currentLapTime = 0f;

        // Проверяем завершение гонки
        if (currentLap > totalLaps)
        {
            FinishRace();
        }

        UpdateUIText();
    }

    private void FinishRace()
    {
        isRacing = false;
        Debug.Log("Гонка завершена! Лучшее время: " + FormatTime(bestLapTime));
        SceneManager.LoadScene(0);
    }

    private void UpdateUIText()
    {
        if (currentLapTimeText != null)
            currentLapTimeText.text = "Текущий круг: " + FormatTime(currentLapTime);

        if (bestLapTimeText != null)
            bestLapTimeText.text = "Лучший круг: " + (bestLapTime == Mathf.Infinity ? "--:--.---" : FormatTime(bestLapTime));

        if (lapCounterText != null)
            lapCounterText.text = $"Круг {currentLap}/{totalLaps}";
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);

        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    // Методы для получения информации
    public float GetCurrentLapTime() => currentLapTime;
    public float GetBestLapTime() => bestLapTime == Mathf.Infinity ? 0f : bestLapTime;
    public int GetCurrentLap() => currentLap;
    public bool IsRacing() => isRacing;
}