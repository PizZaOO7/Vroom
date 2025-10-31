using LogitechG29.Sample.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttoncontroller : MonoBehaviour
{
    [SerializeField] InputControllerReader inputActions;
    [SerializeField] float a;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Startgame()
    {
        SceneManager.LoadScene(1);
    }
    void Endgame()
    {
        Application.Quit();
    }
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Throttle >= 0.5f) { Startgame(); }
        if (inputActions.Brake >= 0.5f) { Endgame(); }
        a = inputActions.Throttle;
    }
}
