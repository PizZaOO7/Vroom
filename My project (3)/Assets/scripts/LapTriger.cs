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
            
            if (lapTimer.GetCurrentLap() > 0) // �� ��������� �� ����� ������
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
        yield return new WaitForSeconds(2f); // ������ �� ���������� ������������
        canTrigger = true;
    }
}