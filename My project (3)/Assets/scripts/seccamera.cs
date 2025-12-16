using UnityEngine;

public class seccamera : MonoBehaviour
{

    [SerializeField] private Transform Transform;

    private Vector3 offset = new Vector3(0f, 2f, -4f);
    private float speed = 10f;
    void Update()
    {
        var targetPosition = transform.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed*Time.deltaTime);
        var direction = transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation,rotation,speed * Time.deltaTime);
    }
}
