#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    int[] qwe = new int[3] { 100, 0, 0 };
    int[] glove = new int[3] { 100, 100, 100 };
    int a = 0;
    [SerializeField]
    private float maxMotorTorque; // максимальный крутящий момент, который двигатель может приложить к колесу

    [SerializeField] private float maxSteeringAngle; // максимальный угол поворота, который может иметь колесо
    
    public AudioSource brakeSource;
    public AudioSource speedAudioSource;
    public void FixedUpdate()
    {
        var speed = 0f;
        if (inputControllerReader.Throttle != 0)
        {
            speed = -inputControllerReader.Throttle;
        }
        else if (inputControllerReader.Brake != 0)
        {
            speed = inputControllerReader.Brake;

        }

        var motor = maxMotorTorque * speed;
        var steering = maxSteeringAngle * inputControllerReader.Steering;

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
            BhapticsLibrary.Play("boom", 0, 100, 0, 0, 0);
        }
    }

    public void perevorot(Transform transform)
    {   
        transform.position += new Vector3(0,2,0);
        transform.rotation = Quaternion.AngleAxis(0,new Vector3(0,0,1));
        BhapticsLibrary.Play("boom", 0, 40, 0, 0, 0);
    }

    private void Update()
    {
        if (inputControllerReader.Brake > 0.5)
        {
            Debug.Log("brake");
            BhapticsLibrary.Play("brake", 0, 40, 0, 0,0);
            
        }


        if (inputControllerReader.EastButton)
        {   
            
            if (audioSources[a].isPlaying) { audioSources[a].Stop(); a=(a+1)%audioSources.Length; }
                
            BhapticsLibrary.PlayMotors((int)PositionType.GloveL, qwe, 0);
            audioSources[a].Play();
            
            Debug.Log("музыка");
        }
        if (currentSpeed > 40)
        {
            BhapticsLibrary.PlayMotors((int)PositionType.GloveL, glove, 0);
            BhapticsLibrary.PlayMotors((int)PositionType.GloveR, glove, 0);
            Debug.Log("высокая скорость");
        }
        if (inputControllerReader.LeftShift) { perevorot(transform); }
        if (inputControllerReader.RightShift) { SceneManager.LoadScene(0); }
    }

}