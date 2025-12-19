using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LapTimer : MonoBehaviour
{
    [Header("Настройки")]
    public int totalLaps = 3;
    public GUIStyle timerStyle;
    public GUIStyle lapStyle;
    public GUIStyle bestStyle;

    private float currentLapTime = 0f;
    private float bestLapTime = Mathf.Infinity;
    private int currentLap = 1;
    private bool isRacing = false;

    void Start()
    {
        StartRace();

        // Создаем стандартные стили если они не заданы
        if (timerStyle == null)
        {
            timerStyle = new GUIStyle();
            timerStyle.fontSize = 24;
            timerStyle.normal.textColor = Color.white;
            timerStyle.fontStyle = FontStyle.Bold;
            timerStyle.alignment = TextAnchor.UpperLeft;
        }

        if (lapStyle == null)
        {
            lapStyle = new GUIStyle();
            lapStyle.fontSize = 20;
            lapStyle.normal.textColor = Color.yellow;
            lapStyle.alignment = TextAnchor.UpperLeft;
        }

        if (bestStyle == null)
        {
            bestStyle = new GUIStyle();
            bestStyle.fontSize = 20;
            bestStyle.normal.textColor = Color.green;
            bestStyle.alignment = TextAnchor.UpperLeft;
        }
    }

    void Update()
    {
        if (isRacing)
        {
            currentLapTime += Time.deltaTime;
        }
    }

    void OnGUI()
    {
        if (!isRacing) return;

        // Отступ от края экрана
        float xPos = 1000;
        float yPos = 10;
        float lineHeight = 30;

        // Отображаем текущий круг
        GUI.Label(new Rect(xPos, yPos, 300, lineHeight),
                 $"КРУГ: {currentLap}/{totalLaps}", lapStyle);

        // Отображаем текущее время круга
        yPos += lineHeight;
        GUI.Label(new Rect(xPos, yPos, 300, lineHeight),
                 $"ТЕКУЩИЙ: {FormatTime(currentLapTime)}", timerStyle);

        // Отображаем лучшее время
        yPos += lineHeight;
        string bestTimeText = bestLapTime == Mathf.Infinity ?
            "ЛУЧШИЙ: --:--.---" :
            $"ЛУЧШИЙ: {FormatTime(bestLapTime)}";
        GUI.Label(new Rect(xPos, yPos, 300, lineHeight), bestTimeText, bestStyle);

        
    }

    public void StartRace()
    {
        isRacing = true;
        currentLap = 1;
        currentLapTime = 0f;
        bestLapTime = Mathf.Infinity;
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
    }

    private void FinishRace()
    {
        isRacing = false;
        SceneManager.LoadScene(0);
        // Показываем финальное сообщение
        //ShowFinishMessage();
    }

    //private void ShowFinishMessage()
    //{
    //    // Используем корутину для отображения сообщения
    //    StartCoroutine(DisplayFinishMessage());
    //}

    //private IEnumerator DisplayFinishMessage()
    //{
    //    float displayTime = 5f;
    //    float elapsedTime = 0f;

    //    while (elapsedTime < displayTime)
    //    {
    //        // Отображаем сообщение о завершении гонки
    //        Rect messageRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100);
    //        GUIStyle messageStyle = new GUIStyle();
    //        messageStyle.fontSize = 32;
    //        messageStyle.normal.textColor = Color.yellow;
    //        messageStyle.alignment = TextAnchor.MiddleCenter;
    //        messageStyle.fontStyle = FontStyle.Bold;

    //        GUI.Label(messageRect, "ГОНКА ЗАВЕРШЕНА!", messageStyle);

    //        // Отображаем лучшее время
    //        Rect timeRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 + 20, 300, 50);
    //        GUIStyle timeStyle = new GUIStyle();
    //        timeStyle.fontSize = 24;
    //        timeStyle.normal.textColor = Color.green;
    //        timeStyle.alignment = TextAnchor.MiddleCenter;

    //        GUI.Label(timeRect, $"Лучший круг: {FormatTime(bestLapTime)}", timeStyle);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //}

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