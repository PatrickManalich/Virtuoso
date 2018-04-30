using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManagerScript : MonoBehaviour {

    private int selectedDummyIndex;                 // The index for the currently selected dummy in dummies
    private int sampleCount;                        // The sample count based on the animation seconds
    private GameObject[] dummies;                   // The array of dummies in the scene
    private GameObject[] startAids;                 // The array of start aids in the scene
    private GameObject[] endAids;                   // The array of end aids in the scene
    private GameObject[] ghosts;                    // The array of ghosts in the scene
    private SpeedBandScript speedBandScript;        // The speed band script, used for caching
    private PlayBandScript playBandScript;          // The play band script, used for caching
    private SliderFieldScript sliderFieldScript;    // The slider field script, used for caching
    private GloveScript leftGloveScript;            // The glove script of the left glove, used for caching
    private RefineGuideScript refineGuideScript;    // The refine guide script, used for caching
    private OVRGrabber leftOVRGrabberScript;        // The OVR grabber script of the left hand anchor
    private OVRGrabber rightOVRGrabberScript;       // The OVR grabber script of the right hand anchor

    public Material startAidMaterial;               // The start aid material
    public Material endAidMaterial;                 // The end aid material
    public Material ghostMaterial;                  // The ghost material
    public float animationSeconds;                  // How long the animation lasts in seconds
    public GameObject speedBand;                    // The speed band of the slider field
    public GameObject playBand;                     // The play band of the slider field
    public GameObject sliderField;                  // The slider field Game Object, used for adjusting current slider
    public GameObject leftHandAnchor;               // The left hand anchor Game Object
    public GameObject rightHandAnchor;              // The right hand anchor Game Object
    public GameObject leftGlove;                    // The left glove Game Object (you only need one)
    public GameObject refineGuide;                  // The refine guide Game Object
    public GameObject dummyIndicator;

    private void Awake() {
            // Initialize all private scripts
        speedBandScript = speedBand.GetComponent<SpeedBandScript>();
        playBandScript = playBand.GetComponent<PlayBandScript>();
        sliderFieldScript = sliderField.GetComponent<SliderFieldScript>();
        leftGloveScript = leftGlove.GetComponent<GloveScript>();
        refineGuideScript = refineGuide.GetComponent<RefineGuideScript>();
        leftOVRGrabberScript = leftHandAnchor.GetComponent<OVRGrabber>();
        rightOVRGrabberScript = rightHandAnchor.GetComponent<OVRGrabber>();
            // Create arrays based on number of dummies in the scene
        dummies = GameObject.FindGameObjectsWithTag("Dummy");
        startAids = new GameObject[dummies.Length];
        endAids = new GameObject[dummies.Length];
        ghosts = new GameObject[dummies.Length];

        for (int i = 0; i < dummies.Length; i++) {
            DummyManagerScript DMS = gameObject.GetComponent<DummyManagerScript>();
                // Instantiate aids and ghosts
            GameObject dummy = dummies[i];
            GameObject startAid = Instantiate(dummy);
            GameObject endAid = Instantiate(dummy);
            GameObject ghost = Instantiate(dummy);
            float newScalar = dummy.transform.localScale.x * 0.999f; // Prevents mesh clashing when overlaid
            Vector3 newLocalScale = new Vector3(newScalar, newScalar, newScalar);
                // Add components to each dummy
            dummy.AddComponent<DummyScript>();
            sampleCount = (int)Mathf.Ceil(animationSeconds / speedBandScript.GetSampleRate()); // Ceil to make a nice, even number
            dummy.GetComponent<DummyScript>().Initialize(ref DMS, sampleCount);
            dummy.AddComponent<Rigidbody>();
            dummy.GetComponent<Rigidbody>().useGravity = false;
            dummy.GetComponent<Rigidbody>().isKinematic = true;
            dummy.AddComponent<BoxCollider>();
                // Edit start aid components
            startAid.name = dummy.name + "StartAid";
            startAid.transform.localScale = newLocalScale;
            startAid.GetComponent<Renderer>().material = startAidMaterial;
            startAids[i] = startAid;
                // Edit end aid components
            endAid.name = dummy.name + "EndAid";
            endAid.transform.localScale = newLocalScale;
            endAid.GetComponent<Renderer>().material = endAidMaterial;
            endAids[i] = endAid;
                // Edit ghost components
            ghost.AddComponent<GhostScript>();
            ghost.GetComponent<GhostScript>().Initialize(ref DMS, ref sliderFieldScript, ref speedBandScript, i);
            ghost.name = dummy.name + "Ghost";
            ghost.transform.localScale = newLocalScale;
            ghost.GetComponent<Renderer>().material = ghostMaterial;
            ghosts[i] = ghost;
        }
            // Set the selected dummy to the first dummy found, add the OVRGrabble script (so only the selected dummy can be grabbed),
            // and parent dummy indicator with the selected dummy and place it above the dummy
        selectedDummyIndex = 0;
        dummies[selectedDummyIndex].AddComponent<OVRGrabbable>();
        dummyIndicator.transform.parent = dummies[selectedDummyIndex].transform;
        dummyIndicator.transform.localPosition = new Vector3(0.0f, dummies[selectedDummyIndex].GetComponent<BoxCollider>().size.y / 2, 0.0f);
        dummyIndicator.transform.Rotate(90f, 0f, 0f);
        dummyIndicator.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.GetDown(OVRInput.Button.One)) {
                // Remove OVRGrabble script from currently selected dummy, move to next dummy, and adjust dummy indicator
            Destroy(dummies[selectedDummyIndex].GetComponent<OVRGrabbable>());
            selectedDummyIndex++;
            if (selectedDummyIndex > dummies.Length - 1)
                selectedDummyIndex = 0;
            dummies[selectedDummyIndex].AddComponent<OVRGrabbable>();
            dummyIndicator.transform.parent = dummies[selectedDummyIndex].transform;
            dummyIndicator.transform.localPosition = new Vector3(0.0f, dummies[selectedDummyIndex].GetComponent<BoxCollider>().size.y / 2, 0.0f);
        }
    }

    /* Functions from Dummy Manager Script. No initials required. */
    public GameObject GetSelectedDummy() { return dummies[selectedDummyIndex]; }
    public int GetSampleCount() { return sampleCount; }
    public void AdjustStartAids(int sampleIndex) {
        for(int i = 0; i < dummies.Length; i++) {
            startAids[i].transform.position = dummies[i].GetComponent<DummyScript>().GetSamplePosition(sampleIndex);
            startAids[i].transform.rotation = dummies[i].GetComponent<DummyScript>().GetSampleRotation(sampleIndex);
        }
    }
    public void AdjustEndAids(int sampleIndex) {
        for (int i = 0; i < dummies.Length; i++) {
            endAids[i].transform.position = dummies[i].GetComponent<DummyScript>().GetSamplePosition(sampleIndex);
            endAids[i].transform.rotation = dummies[i].GetComponent<DummyScript>().GetSampleRotation(sampleIndex);
        }
    }

    /* Functions from other scripts. Initials in front of function name represent the script's name. For example, SFS = SliderFieldScript. */
    public void SFS_AdjustSlider(string sliderName, int sampleIndex, float t) { sliderFieldScript.AdjustSlider(sliderName, sampleIndex, t); }
    public int SFS_GetStartSliderSampleIndex() { return sliderFieldScript.GetStartSliderSampleIndex(); }
    public int SFS_GetEndSliderSampleIndex() { return sliderFieldScript.GetEndSliderSampleIndex(); }
    public void SFS_SetGhostSliderActive(bool activeSelf) { sliderFieldScript.SetGhostSliderActive(activeSelf); }
    public float SBS_GetSampleRate() { return speedBandScript.GetSampleRate(); }
    public void OVRG_ForceRelease(OVRGrabbable gameObjectOVRGrabbable) {
        leftOVRGrabberScript.ForceRelease(gameObjectOVRGrabbable);
        rightOVRGrabberScript.ForceRelease(gameObjectOVRGrabbable);
    }
    public void PBS_ForcePauseToggle() { playBandScript.ForcePauseToggle(); }
    public bool LGS_IsInOverwriteState() { return leftGloveScript.isInOverwriteState(); }
    public void RGS_StartGuiding(Vector3 newAnchorPoint) { refineGuideScript.StartGuiding(newAnchorPoint); }
    public void RGS_StopGuiding() { refineGuideScript.StopGuiding(); }
    public void GS_StartGhosting() {
        foreach (GameObject ghost in ghosts) { ghost.GetComponent<GhostScript>().StartGhosting(); }
    }
    public void GS_StopGhosting() {
        foreach (GameObject ghost in ghosts) { ghost.GetComponent<GhostScript>().StopGhosting(); }
    }

    /* Functions from Dummy Script. DS stands for DummyScript */
    public Vector3 DS_GetSamplePosition(int personalDummyIndex, int sampleIndex) {
        return dummies[personalDummyIndex].GetComponent<DummyScript>().GetSamplePosition(sampleIndex);
    }
    public Quaternion DS_GetSampleRotation(int personalDummyIndex, int sampleIndex) {
        return dummies[personalDummyIndex].GetComponent<DummyScript>().GetSampleRotation(sampleIndex);
    }
    public void DS_Adjust(int sampleIndex, float t) {
        foreach (GameObject dummy in dummies) { dummy.GetComponent<DummyScript>().Adjust(sampleIndex, t); }
    }
    public void DS_StartPlaying() {
        foreach (GameObject dummy in dummies) { dummy.GetComponent<DummyScript>().StartPlaying(); }
    }
    public void DS_StopPlaying() {
        foreach (GameObject dummy in dummies) { dummy.GetComponent<DummyScript>().StopPlaying(); }
    }
    public void DS_GoToStart() {
        sliderFieldScript.AdjustSlider("CurrentSlider", sliderFieldScript.GetStartSliderSampleIndex(), 0);
        foreach (GameObject dummy in dummies) { dummy.GetComponent<DummyScript>().GoToStart(); }
    }
    public void DS_AttemptGrabBegin(GameObject attemptDummy) {
        if(String.Equals(attemptDummy.name, dummies[selectedDummyIndex].name)) {
            dummies[selectedDummyIndex].GetComponent<DummyScript>().GrabBegin();
            foreach (GameObject dummy in dummies) {
                if (!String.Equals(attemptDummy.name, dummy.name))
                    dummy.GetComponent<DummyScript>().StartPlaying();
            }
        }
    }
    public void DS_AttemptGrabEnd(GameObject attemptDummy) {
        if (String.Equals(attemptDummy.name, dummies[selectedDummyIndex].name)) {
            dummies[selectedDummyIndex].GetComponent<DummyScript>().GrabEnd();
            foreach (GameObject dummy in dummies) {
                if (!String.Equals(attemptDummy.name, dummy.name))
                    dummy.GetComponent<DummyScript>().StopPlaying();
            }
        }
    }
    public void DS_AlternateSamples() {
        dummies[selectedDummyIndex].GetComponent<DummyScript>().AlternateSamples();
        sliderFieldScript.AdjustSlider("EndSlider", sliderFieldScript.GetEndSliderSampleIndex(), 0f);

    }
}