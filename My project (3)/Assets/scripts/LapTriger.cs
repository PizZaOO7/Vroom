using UnityEngine;

public class StartFinishLine : MonoBehaviour
{
    public LapTimer lapTimer;
    private bool canTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("Player"))
        {
            
            if (lapTimer.GetCurrentLap() > 0) // Не триггерим на самом старте
            {
                Debug.Log("11111111111111111111111111111111111111111111111111111");
                lapTimer.CompleteLap();
                StartCoroutine(Cooldown());
                
            }
        }
    }

    private System.Collections.IEnumerator Cooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(2f); // Защита от повторного срабатывания
        canTrigger = true;
    }
}