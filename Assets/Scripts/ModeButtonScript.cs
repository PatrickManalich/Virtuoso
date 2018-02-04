using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeButtonScript : MonoBehaviour {

    public GameObject toy;
    public GameObject playButton;
    public GameObject playButtonText;
    public GameObject playSlider;
    public GameObject editButton;
    public GameObject editButtonText;
    public GameObject editBeginSlider;
    public GameObject editEndSlider;
    public GameObject editBeginAid;
    public GameObject editEndAid;
    public GameObject playModeText;
    private string mode;

    void Start() {
        playButton.SetActive(true);
        playButtonText.SetActive(true);
        playButtonText.GetComponent<TextMesh>().text = "||";
        playSlider.SetActive(true);
        playModeText.SetActive(true);
        playModeText.GetComponent<TextMesh>().text = "Play Mode";
        mode = "Play";
    }

    void Update() { }

    void OnMouseDown() {
        if(mode == "Play") {        // Switch to Edit Mode
            playButton.SetActive(false);
            playButtonText.SetActive(false);
            editButton.SetActive(true);
            editButtonText.SetActive(true);
            editButtonText.GetComponent<TextMesh>().text = "Ghost\n  Off";
            playSlider.SetActive(false);
            editBeginSlider.SetActive(true);
            editEndSlider.SetActive(true);
            editBeginAid.SetActive(true);
            editEndAid.SetActive(true);
            playModeText.GetComponent<TextMesh>().text = "Edit Mode";
            mode = "Edit";
        } else if(mode == "Edit") { // Switch to Play Mode
            editButton.SetActive(false);
            editButtonText.SetActive(false);
            playButton.SetActive(true);
            playButtonText.SetActive(true);
            playButtonText.GetComponent<TextMesh>().text = "||";
            editBeginSlider.SetActive(false);
            editEndSlider.SetActive(false);
            editBeginAid.SetActive(false);
            editEndAid.SetActive(false);
            playSlider.SetActive(true);
            playModeText.GetComponent<TextMesh>().text = "Play Mode";
            mode = "Play";
        }

    }

}