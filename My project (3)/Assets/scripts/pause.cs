using LogitechG29.Sample.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    [Header("Настройки паузы")]
    
    public bool canPause = true;

    [SerializeField] private InputControllerReader inputControllerReader;

    

    

    private bool isPaused = false;

    

    void Update()
    {
        if (canPause && inputControllerReader.Share)
        {
            TogglePause();
        }
        if (canPause && inputControllerReader.Home)
        {
            GoToMenu();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!canPause) return;

        isPaused = true;

        // Останавливаем время
        Time.timeScale = 0f;

        // Показываем панель паузы
        
        Debug.Log("Игра на паузе");
    }

    public void ResumeGame()
    {
        isPaused = false;

        // Возвращаем нормальное время
        Time.timeScale = 1f;

        // Скрываем панель паузы
       

        Debug.Log("Игра продолжена");
    }

    void GoToMenu()
    {
        ResumeGame(); // Снимаем паузу перед переходом
        SceneManager.LoadScene(0);
    }

    // Для UI кнопок
    public void UI_TogglePause()
    {
        TogglePause();
    }

    public void UI_Pause()
    {
        PauseGame();
    }

    public void UI_Resume()
    {
        ResumeGame();
    }

    void OnGUI()
    {
        if (isPaused)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.fontSize = 24;
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Box(new Rect(Screen.width / 2 - 150, 50, 300, 60), "ПАУЗА", style);
        }
    }
}