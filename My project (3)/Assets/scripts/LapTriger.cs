using UnityEngine;
using Bhaptics.SDK2;
using Unity.VisualScripting;

public class StartFinishLine : MonoBehaviour
{

    int[] zxc = new int[16] {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100};
    GlovePlayTime[] playTimeValues = new GlovePlayTime[16] {
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS,
            GlovePlayTime.ThirtyMS, GlovePlayTime.ThirtyMS };
    GloveShapeValue[] shapeValues = new GloveShapeValue[16] {
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant,
            GloveShapeValue.Constant, GloveShapeValue.Constant, };

    public LapTimer lapTimer;
    private bool canTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        BhapticsLibrary.PlayWaveform((int)PositionType.Vest,zxc,playTimeValues,shapeValues);
        BhapticsLibrary.PlayMotors((int)PositionType.FootL, zxc, 100);
        BhapticsLibrary.PlayMotors((int)PositionType.FootL, zxc, 100);
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