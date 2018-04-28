using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManagerScript : MonoBehaviour {

    private int selectedDummyIndex;
    private DummyScript dummyScript;
    private int sampleCount;
    private GameObject[] dummies;
    private GameObject[] startAids;
    private GameObject[] endAids;
    private GameObject[] ghosts;
    private SpeedBandScript speedBandScript;        // The speed band script, used for caching
    private PlayBandScript playBandScript;          // The play band script, used for caching
    private SliderFieldScript sliderFieldScript;    // The slider field script, used for caching
    private GloveScript leftGloveScript;            // The glove script of the left glove, used for caching
    private RefineGuideScript refineGuideScript;    // The refine guide script, used for caching
    private OVRGrabber leftOVRGrabberScript;        // The OVR grabber script of the left hand anchor
    private OVRGrabber rightOVRGrabberScript;       // The OVR grabber script of the right hand anchor

    public Material startAidMaterial;
    public Material endAidMaterial;
    public Material ghostMaterial;
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
        speedBandScript = speedBand.GetComponent<SpeedBandScript>();
        playBandScript = playBand.GetComponent<PlayBandScript>();
        sliderFieldScript = sliderField.GetComponent<SliderFieldScript>();
        leftGloveScript = leftGlove.GetComponent<GloveScript>();
        refineGuideScript = refineGuide.GetComponent<RefineGuideScript>();
        leftOVRGrabberScript = leftHandAnchor.GetComponent<OVRGrabber>();
        rightOVRGrabberScript = rightHandAnchor.GetComponent<OVRGrabber>();

        dummies = GameObject.FindGameObjectsWithTag("Dummy");
        startAids = new GameObject[dummies.Length];
        endAids = new GameObject[dummies.Length];
        ghosts = new GameObject[dummies.Length];

        for (int i = 0; i < dummies.Length; i++) {
            DummyManagerScript DMS = gameObject.GetComponent<DummyManagerScript>();

            GameObject dummy = dummies[i];
            GameObject startAid = Instantiate(dummy);
            GameObject endAid = Instantiate(dummy);
            GameObject ghost = Instantiate(dummy);
            float newScalar = dummy.transform.localScale.x * 0.999f; // Prevents mesh clashing when overlaid
            Vector3 newLocalScale = new Vector3(newScalar, newScalar, newScalar);

            dummy.AddComponent<DummyScript>();
            sampleCount = (int)Mathf.Ceil(animationSeconds / DMS.SBC_GetSampleRate()); // Ceil to make a nice, even number
            dummy.GetComponent<DummyScript>().Initialize(ref DMS, sampleCount);
            dummy.AddComponent<Rigidbody>();
            dummy.GetComponent<Rigidbody>().useGravity = false;
            dummy.GetComponent<Rigidbody>().isKinematic = true;
            dummy.AddComponent<BoxCollider>();
            
            startAid.name = dummy.name + "StartAid";
            startAid.transform.localScale = newLocalScale;
            startAid.GetComponent<Renderer>().material = startAidMaterial;
            startAids[i] = startAid;
            
            endAid.name = dummy.name + "EndAid";
            endAid.transform.localScale = newLocalScale;
            endAid.GetComponent<Renderer>().material = endAidMaterial;
            endAids[i] = endAid;
            
            ghost.AddComponent<GhostScript>();
            ghost.GetComponent<GhostScript>().Initialize(ref DMS, ref sliderFieldScript, ref speedBandScript);
            ghost.name = dummy.name + "Ghost";
            ghost.transform.localScale = newLocalScale;
            ghost.GetComponent<Renderer>().material = ghostMaterial;
            ghosts[i] = ghost;
        }

        selectedDummyIndex = 0;
        dummies[selectedDummyIndex].AddComponent<OVRGrabbable>();
        dummyScript = dummies[selectedDummyIndex].GetComponent<DummyScript>();
        dummyIndicator.transform.parent = dummies[selectedDummyIndex].transform;
        dummyIndicator.transform.localPosition = new Vector3(0.0f, dummies[selectedDummyIndex].GetComponent<BoxCollider>().size.y / 2, 0.0f);
        dummyIndicator.transform.Rotate(90f, 0f, 0f);
        dummyIndicator.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.GetDown(OVRInput.Button.One)) {
            Destroy(dummies[selectedDummyIndex].GetComponent<OVRGrabbable>());
            selectedDummyIndex++;
            if (selectedDummyIndex > dummies.Length - 1)
                selectedDummyIndex = 0;
            dummies[selectedDummyIndex].AddComponent<OVRGrabbable>();
            dummyScript = dummies[selectedDummyIndex].GetComponent<DummyScript>();
            dummyIndicator.transform.parent = dummies[selectedDummyIndex].transform;
            dummyIndicator.transform.localPosition = new Vector3(0.0f, dummies[selectedDummyIndex].GetComponent<BoxCollider>().size.y / 2, 0.0f);
        }
    }

    /* Functions from Dummy Manager Script for any script to use */
    public GameObject GetDummy() { return dummies[selectedDummyIndex]; }
    public GameObject GetStartAid() { return startAids[selectedDummyIndex]; }
    public GameObject GetEndAid() { return endAids[selectedDummyIndex]; }
    public GameObject GetGhost() { return ghosts[selectedDummyIndex]; }
    public int GetSampleCount() { return sampleCount; }

    /* Functions from other scripts for Dummy Script to use */
    public void SFS_AdjustSlider(string sliderName, int sampleIndex, float t) { sliderFieldScript.AdjustSlider(sliderName, sampleIndex, t); }
    public int SFS_GetStartSliderSampleIndex() { return sliderFieldScript.GetStartSliderSampleIndex(); }
    public int SFS_GetEndSliderSampleIndex() { return sliderFieldScript.GetEndSliderSampleIndex(); }
    public float SBC_GetSampleRate() { return speedBandScript.GetSampleRate(); }
    public void OVRG_ForceRelease(OVRGrabbable gameObjectOVRGrabbable) {
        leftOVRGrabberScript.ForceRelease(gameObjectOVRGrabbable);
        rightOVRGrabberScript.ForceRelease(gameObjectOVRGrabbable);
    }
    public void PBS_ForcePauseToggle() { playBandScript.ForcePauseToggle(); }
    public bool LGS_IsInOverwriteState() { return leftGloveScript.isInOverwriteState(); }
    public void RGS_StartGuiding(Vector3 newAnchorPoint) { refineGuideScript.StartGuiding(newAnchorPoint); }
    public void RGS_StopGuiding() { refineGuideScript.StopGuiding(); }
    public void GS_StartGhosting() { ghosts[selectedDummyIndex].GetComponent<GhostScript>().StartGhosting(); }  // Exceptions, used by Ghost Band Script
    public void GS_StopGhosting() { ghosts[selectedDummyIndex].GetComponent<GhostScript>().StopGhosting(); }    // instead of Dummy Script

    /* Functions from Dummy Script for other scripts to use */
    public Vector3 DS_GetSamplePosition(int sampleIndex) { return dummyScript.GetSamplePosition(sampleIndex); }
    public Quaternion DS_GetSampleRotation(int sampleIndex) { return dummyScript.GetSampleRotation(sampleIndex); }
    public void DS_Adjust(int sampleIndex, float t) { dummyScript.Adjust(sampleIndex, t); }
    public void DS_StartPlaying() { dummyScript.StartPlaying(); }
    public void DS_StopPlaying() { dummyScript.StopPlaying(); }
    public void DS_GoToStart() { dummyScript.GoToStart(); }
    public void DS_AttemptGrabBegin(GameObject gameObject) {
        if(String.Equals(gameObject.name, dummies[selectedDummyIndex].name))
            dummyScript.GrabBegin();
    }
    public void DS_AttemptGrabEnd(GameObject gameObject) {
        if (String.Equals(gameObject.name, dummies[selectedDummyIndex].name))
            dummyScript.GrabEnd();
    }
}