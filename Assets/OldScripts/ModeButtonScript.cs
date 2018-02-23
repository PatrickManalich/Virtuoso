using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeButtonScript : MonoBehaviour {

    private enum Mode { Play, Edit };
    private Mode mode;
    public GameObject toy;
    private ToyScript toyScript;
    public GameObject playButton;
    private PlayButtonScript playButtonScript;
    public GameObject playButtonText;
    public GameObject playSlider;
    private PlaySliderScript playSliderScript;
    public GameObject editGhostButton;
    private EditGhostButtonScript editGhostButtonScript;
    public GameObject editGhostButtonText;
    public GameObject editBeginSlider;
    public GameObject editEndSlider;
    public GameObject editBeginAid;
    public GameObject editEndAid;
    public GameObject editSliderController;
    private EditSliderControllerScript editSliderControllerScript;
    public GameObject modeButtonText;

    void Awake() {
        toyScript = toy.GetComponent<ToyScript>();
        playButtonScript = playButton.GetComponent<PlayButtonScript>();
        playSliderScript = playSlider.GetComponent<PlaySliderScript>();
        editGhostButtonScript = editGhostButton.GetComponent<EditGhostButtonScript>();
        editSliderControllerScript = editSliderController.GetComponent<EditSliderControllerScript>();
        playButton.SetActive(true);
        playButtonText.SetActive(true);
        playSlider.SetActive(true);
        modeButtonText.SetActive(true);
        modeButtonText.GetComponent<TextMesh>().text = "Play Mode";
        toyScript.isInEditMode = false;
        mode = Mode.Play;
    }

    void OnMouseDown() {
        if(mode == Mode.Play) {        // Switch to Edit Mode
            if (toyScript.isAnimationRecorded) {
                playButtonScript.ChangeState(false);
                playButton.SetActive(false);
                playButtonText.SetActive(false);
                playSlider.SetActive(false);
                editGhostButton.SetActive(true);
                editGhostButtonText.SetActive(true);
                editBeginSlider.SetActive(true);
                editEndSlider.SetActive(true);
                editBeginAid.SetActive(true);
                editEndAid.SetActive(true);
                editSliderController.SetActive(true);
                editSliderControllerScript.SetupLocations();
                modeButtonText.GetComponent<TextMesh>().text = "Edit Mode";
                toyScript.isInEditMode = true;
                mode = Mode.Edit;
            } else
                Debug.Log("No animation has been recorded");
        } else if(mode == Mode.Edit) { // Switch to Play Mode
            editGhostButtonScript.ChangeState(false);
            editGhostButton.SetActive(false);
            editGhostButtonText.SetActive(false);
            editBeginSlider.SetActive(false);
            editEndSlider.SetActive(false);
            editBeginAid.SetActive(false);
            editEndAid.SetActive(false);
            editSliderController.SetActive(false);
            playButton.SetActive(true);
            playButtonText.SetActive(true);
            playSlider.SetActive(true);
            playSliderScript.SetupLocations();
            modeButtonText.GetComponent<TextMesh>().text = "Play Mode";
            toyScript.isInEditMode = false;
            mode = Mode.Play;
        }

    }

}