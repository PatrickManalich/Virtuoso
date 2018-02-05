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
    public GameObject editGhostButton;
    private EditGhostButtonScript editGhostButtonScript;
    public GameObject editGhostButtonText;
    public GameObject editBeginSlider;
    public GameObject editEndSlider;
    public GameObject editSliderController;
    public GameObject modeText;

    void Start() {
        toyScript = toy.GetComponent<ToyScript>();
        playButtonScript = playButton.GetComponent<PlayButtonScript>();
        editGhostButtonScript = editGhostButton.GetComponent<EditGhostButtonScript>();
        playButton.SetActive(true);
        playButtonText.SetActive(true);
        playSlider.SetActive(true);
        modeText.SetActive(true);
        modeText.GetComponent<TextMesh>().text = "Play Mode";
        mode = Mode.Play;
    }

    void Update() { }

    void OnMouseDown() {
        if(mode == Mode.Play) {        // Switch to Edit Mode
            if (toyScript.AnimationRecorded()) {
                playButtonScript.ChangeState(false);
                playButton.SetActive(false);
                playButtonText.SetActive(false);
                playSlider.SetActive(false);
                editGhostButton.SetActive(true);
                editGhostButtonText.SetActive(true);
                editBeginSlider.SetActive(true);
                editEndSlider.SetActive(true);
                editSliderController.SetActive(true);
                modeText.GetComponent<TextMesh>().text = "Edit Mode";
                mode = Mode.Edit;
            } else
                Debug.Log("No animation has been recorded");
        } else if(mode == Mode.Edit) { // Switch to Play Mode
            editGhostButtonScript.ChangeState(false);
            editGhostButton.SetActive(false);
            editGhostButtonText.SetActive(false);
            editBeginSlider.SetActive(false);
            editEndSlider.SetActive(false);
            editSliderController.SetActive(false);
            playButton.SetActive(true);
            playButtonText.SetActive(true);
            playSlider.SetActive(true);
            modeText.GetComponent<TextMesh>().text = "Play Mode";
            mode = Mode.Play;
        }

    }

}