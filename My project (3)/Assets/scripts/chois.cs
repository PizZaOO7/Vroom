using UnityEngine;
using UnityEngine.SceneManagement;

public class chois : MonoBehaviour
{

    
    [SerializeField] GameObject Object1;
    [SerializeField] GameObject Object2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        int index = PlayerPrefs.GetInt("SelectedCar", 0);
        if (index == 0)
        {
            Object1.SetActive(true);
            Debug.Log($"машина {index}");
        }
        else 
        {
            Object2.SetActive(true);
            Debug.Log($"машина {index}");
        }
    }
    //int GetSelectedCarIndex()
    //{
        

    //    // Способ 2: Из PlayerPrefs (более надежно)
    //    int fromPlayerPrefs = PlayerPrefs.GetInt("SelectedCarIndex", 0);

    //    // Возвращаем значение из PlayerPrefs если оно есть, иначе из статического поля
    //    return fromPlayerPrefs;
    //}

}
