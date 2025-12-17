#region

using System;
using System.Collections.Generic;
using Bhaptics.SDK2;
using LogitechG29.Sample.Input;
using UnityEngine;
using UnityEngine.SceneManagement;


#endregion

public class CarControllerSample : MonoBehaviour
{
    [SerializeField] private Transform Transform;
    private Rigidbody carRigidbody ;
    [SerializeField] private InputControllerReader inputControllerReader;
    [SerializeField] private List<AxleInfo> axleInfos; // информация о каждой отдельной оси
    [SerializeField] private float currentSpeed;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;

    int[] qwe = new int[3] { 100, 0, 0 };
    int[] glove = new int[3] { 100, 100, 100 };
    int[] vest = new int[16] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };
    int a = 0;
    float time;
    float time1;
    float time2;
    [SerializeField]
    private float maxMotorTorque; // максимальный крутящий момент, который двигатель может приложить к колесу

    [SerializeField] private float maxSteeringAngle; // максимальный угол поворота, который может иметь колесо
    
    public AudioSource brakeSource;
    public AudioSource speedAudioSource;

    [SerializeField] float adjustedMotorForce;
    [SerializeField] float adjustedSteerAngle;

    public SimpleTireWearController tireWearController;
 
    public void FixedUpdate()
    {
        var speed = 0f;

        if (tireWearController != null)
        {
            float traction = tireWearController.GetTractionMultiplier();

            // Уменьшаем силу мотора при изношенных шинах
            adjustedMotorForce = maxMotorTorque * traction;

            // Уменьшаем угол поворота для стабильности
            adjustedSteerAngle = maxSteeringAngle * Mathf.Lerp(0.7f, 1f, traction);

            
        }

        if (inputControllerReader.Throttle != 0)
        {
            speed = inputControllerReader.Throttle;
        }
        else if (inputControllerReader.Brake != 0)
        {
            speed = -inputControllerReader.Brake;

        }

        var motor = adjustedMotorForce * speed;
        var steering = adjustedSteerAngle * inputControllerReader.Steering;

        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
        carRigidbody = GetComponent<Rigidbody>();
        currentSpeed  = carRigidbody.linearVelocity.magnitude;
    }

    [Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // это колесо прикреплено к мотору?
        public bool steering; // применяет ли это колесо угол поворота?
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем столкновение с окружением
        if (collision.gameObject.CompareTag("Environment"))
        {
            Debug.Log("boom");
            BhapticsLibrary.PlayMotors((int)PositionType.Vest,vest,100);
            BhapticsLibrary.Play("boom", 0, 100, 1, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pitstop"))
        {
            Debug.Log("pit");
            
            
            Debug.Log("pitstop");
            perevorot();
            
        }
    }
        
   

    public void perevorot()
    {
        
        transform.position += new Vector3(0,2,0);
        Vector3 z = transform.rotation.eulerAngles;
        
        transform.Rotate(0,0,-(z.z)-360); 
        BhapticsLibrary.PlayMotors((int)PositionType.Vest,vest,100);
    }

    private void Update()
    {
        z = transform.rotation.eulerAngles.z;

        if (inputControllerReader.Brake > 0.5 )
        {
            Debug.Log("brake");
            BhapticsLibrary.Play("brake", 0, 12, 1, 0,0);
            BhapticsLibrary.PlayMotors((int)PositionType.FootL,vest,10);
            BhapticsLibrary.PlayMotors((int)PositionType.FootR,vest,10);
            
        }


        if (inputControllerReader.EastButton && Time.time >= time2 + 0.2f)
        {   
            
            if (audioSources[a].isPlaying) { audioSources[a].Stop(); a=(a+1)%audioSources.Length; }
                
            BhapticsLibrary.PlayMotors((int)PositionType.GloveL, glove, 10);
            audioSources[a].Play();
            time2 = Time.time;
            Debug.Log("музыка");
        }
        if (currentSpeed > 40)
        {
            BhapticsLibrary.PlayMotors((int)PositionType.GloveL, glove, 0);
            BhapticsLibrary.PlayMotors((int)PositionType.GloveR, glove, 0);
            Debug.Log("высокая скорость");
        }
        if (inputControllerReader.LeftShift && Time.time >= time + 0.2f) { perevorot();  time = Time.time; }
        if (inputControllerReader.RightShift && Time.time >= time1 + 0.5f) { SceneManager.LoadScene(0); time1 = Time.time; }
        
    }

}