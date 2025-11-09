using LogitechG29.Sample.Input;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    [SerializeField] private InputControllerReader inputControllerReader;
    [SerializeField] private Camera camers;
    [SerializeField] private XROrigin xROrigin;
    float time;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void switchecamera()
    {
        if (xROrigin.isActiveAndEnabled)
        {
            xROrigin.enabled = false;
            camers.enabled = true;
            Debug.Log("1");
        }
        else 
        {
            xROrigin.enabled = true;
            camers.enabled = false;
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
