using LogitechG29.Sample.Input;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    [SerializeField] private InputControllerReader inputControllerReader;
    [SerializeField] private XROrigin[] xROrigins;
    [SerializeField] private Camera[] camers;
    float time;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void switchecamera()
    {
        if (camers[0].isActiveAndEnabled)
        {
            camers[0].enabled = false;
            camers[1].enabled = true;
            Debug.Log("1");
        }
        else 
        {
            camers[0].enabled = true;
            camers[1].enabled = false;
            Debug.Log("2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputControllerReader.WestButton && Time.time >= time + 0.2f) 
        {
            Debug.Log("4");
            switchecamera();
            time = Time.time;
        }
    }
}
