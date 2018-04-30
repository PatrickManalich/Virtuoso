using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

    private DummyManagerScript DMS;             // The dummy manager script
    private SliderFieldScript sliderFieldScript;    // The slider field script of the slider field Game Object
    private SpeedBandScript speedBandScript;    // The speed band script of the speed band Game Object
    private int personalDummyIndex;             // The index that represents the respective dummy in DMS for this particular ghost

    /* Coroutine that takes in a ghost sample index and begins moving to the next sample index by lerping over t. Will increment the ghost sample index and call itself
     * once t is greater than 1. If the ghost sample index becomes greater than the end slider sample index, will start the ghost from the start slider sample index. */
    private IEnumerator MoveGhostToNextSample(int ghostSampleIndex) {
        if (ghostSampleIndex < sliderFieldScript.GetEndSliderSampleIndex()) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / speedBandScript.GetSampleRate();
                transform.position = Vector3.Lerp(DMS.DS_GetSamplePosition(personalDummyIndex, ghostSampleIndex), DMS.DS_GetSamplePosition(personalDummyIndex, ghostSampleIndex + 1), t);
                transform.rotation = Quaternion.Lerp(DMS.DS_GetSampleRotation(personalDummyIndex, ghostSampleIndex), DMS.DS_GetSampleRotation(personalDummyIndex, ghostSampleIndex + 1), t);
                DMS.SFS_AdjustSlider("GhostSlider", ghostSampleIndex, t);
                yield return null;
            }
            yield return MoveGhostToNextSample(ghostSampleIndex + 1);
        } else {
            transform.position = DMS.DS_GetSamplePosition(personalDummyIndex, sliderFieldScript.GetStartSliderSampleIndex());
            transform.rotation = DMS.DS_GetSampleRotation(personalDummyIndex, sliderFieldScript.GetStartSliderSampleIndex());
            yield return MoveGhostToNextSample(sliderFieldScript.GetStartSliderSampleIndex());
        }
    }

    /* Takes in a dummy manager script, a slider field script, a speed band script, and a personal dummy index and intializes its private variables. 
     * Also sets it inactive initially.*/
    public void Initialize(ref DummyManagerScript DMSParam, ref SliderFieldScript sliderFieldScriptParam, ref SpeedBandScript speedBandScriptParam,
        int personalDummyIndexParam) {
        DMS = DMSParam;
        sliderFieldScript = sliderFieldScriptParam;
        speedBandScript = speedBandScriptParam;
        personalDummyIndex = personalDummyIndexParam;
        gameObject.SetActive(false);
    }

    /* Sets itself active and starts the MoveGhostToNextSample() coroutine. */
    public void StartGhosting() {
        gameObject.SetActive(true);
        DMS.SFS_SetGhostSliderActive(true);
        transform.position = DMS.DS_GetSamplePosition(personalDummyIndex, sliderFieldScript.GetStartSliderSampleIndex());
        transform.rotation = DMS.DS_GetSampleRotation(personalDummyIndex, sliderFieldScript.GetStartSliderSampleIndex());
        StartCoroutine(MoveGhostToNextSample(sliderFieldScript.GetStartSliderSampleIndex()));
    }

    /* Sets itself inactive and stops all coroutines. */
    public void StopGhosting() {
        DMS.SFS_SetGhostSliderActive(false);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
