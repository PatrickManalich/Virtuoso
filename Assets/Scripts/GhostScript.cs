using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

    private DummyManagerScript DMS;
    private SliderFieldScript sliderFieldScript;    // The slider field script of the slider field Game Object
    private SpeedBandScript speedBandScript;    // The speed band script of the speed band Game Object

    /* Coroutine that takes in a ghost sample index and begins moving to the next sample index by lerping over t. Will increment the ghost sample index and call itself
     * once t is greater than 1. If the ghost sample index becomes greater than the end slider sample index, will start the ghost from the start slider sample index. */
    private IEnumerator MoveGhostToNextSample(int ghostSampleIndex) {
        if (ghostSampleIndex < sliderFieldScript.GetEndSliderSampleIndex()) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / speedBandScript.GetSampleRate();
                transform.position = Vector3.Lerp(DMS.DS_GetSamplePosition(ghostSampleIndex), DMS.DS_GetSamplePosition(ghostSampleIndex + 1), t);
                transform.rotation = Quaternion.Lerp(DMS.DS_GetSampleRotation(ghostSampleIndex), DMS.DS_GetSampleRotation(ghostSampleIndex + 1), t);
                yield return null;
            }
            yield return MoveGhostToNextSample(ghostSampleIndex + 1);
        } else {
            transform.position = DMS.DS_GetSamplePosition(sliderFieldScript.GetStartSliderSampleIndex());
            transform.rotation = DMS.DS_GetSampleRotation(sliderFieldScript.GetStartSliderSampleIndex());
            yield return MoveGhostToNextSample(sliderFieldScript.GetStartSliderSampleIndex());
        }
    }

    /* */
    public void Initialize(ref DummyManagerScript DMSParam, ref SliderFieldScript sliderFieldScriptParam, ref SpeedBandScript speedBandScriptParam) {
        DMS = DMSParam;
        sliderFieldScript = sliderFieldScriptParam;
        speedBandScript = speedBandScriptParam;
        gameObject.SetActive(false);
        transform.localScale = DMS.GetDummy().transform.localScale;
    }

    /* Sets itself active and starts the MoveGhostToNextSample() coroutine. */
    public void StartGhosting() {
        gameObject.SetActive(true);
        transform.position = DMS.DS_GetSamplePosition(sliderFieldScript.GetStartSliderSampleIndex());
        transform.rotation = DMS.DS_GetSampleRotation(sliderFieldScript.GetStartSliderSampleIndex());
        StartCoroutine(MoveGhostToNextSample(sliderFieldScript.GetStartSliderSampleIndex()));
    }

    /* Sets itself inactive and stops all coroutines. */
    public void StopGhosting() {
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
