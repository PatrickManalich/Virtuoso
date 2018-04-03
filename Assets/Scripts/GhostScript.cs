using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

    private DummyScript dummyScript;        // The dummy script of the dummy Game Object
    private SliderFieldScript sliderFieldScript;    // The slider field script of the slider field Game Object
    private SpeedBandScript speedBandScript;    // The speed band script of the speed band Game Object

    public GameObject dummy;                // The dummy Game Object
    public GameObject speedBand;            // The speed band Game Object
    public GameObject sliderField;          // The slider field Game Object

    private void Awake() {
        gameObject.SetActive(false);
        dummyScript = dummy.GetComponent<DummyScript>();
        speedBandScript = speedBand.GetComponent<SpeedBandScript>();
        sliderFieldScript = sliderField.GetComponent<SliderFieldScript>();
    }
    

    /* */
    private IEnumerator MoveGhostToNextSample(int ghostSampleIndex) {
        if (ghostSampleIndex < sliderFieldScript.GetEndSliderSampleIndex()) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / speedBandScript.GetSampleRate();
                transform.position = Vector3.Lerp(dummyScript.GetSamplePosition(ghostSampleIndex), dummyScript.GetSamplePosition(ghostSampleIndex + 1), t);
                transform.rotation = Quaternion.Lerp(dummyScript.GetSampleRotation(ghostSampleIndex), dummyScript.GetSampleRotation(ghostSampleIndex + 1), t);
                yield return null;
            }
            yield return MoveGhostToNextSample(ghostSampleIndex + 1);
        } else {
            transform.position = dummyScript.GetSamplePosition(sliderFieldScript.GetStartSliderSampleIndex());
            transform.rotation = dummyScript.GetSampleRotation(sliderFieldScript.GetStartSliderSampleIndex());
            yield return MoveGhostToNextSample(sliderFieldScript.GetStartSliderSampleIndex());
        }
    }

    /* */
    public void StartGhosting() {
        gameObject.SetActive(true);
        transform.position = dummyScript.GetSamplePosition(sliderFieldScript.GetStartSliderSampleIndex());
        transform.rotation = dummyScript.GetSampleRotation(sliderFieldScript.GetStartSliderSampleIndex());
        StartCoroutine(MoveGhostToNextSample(sliderFieldScript.GetStartSliderSampleIndex()));
    }

    /* */
    public void StopGhosting() {
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
