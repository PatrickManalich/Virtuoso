using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyScript : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool playing;
    private float startTime;
    private List<Vector3> pathPoints;
    public float sampleRate;

    void Start () {
        pathPoints = new List<Vector3>();
    }

    void Update() { }

    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        startTime = Time.time;
        pathPoints.Add(transform.position);
    }

    void OnMouseDrag() {
        Vector3 currScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 currPosition = Camera.main.ScreenToWorldPoint(currScreenPoint) + offset;
        transform.position = currPosition;
        if((Time.time - startTime) > sampleRate) {
            startTime = Time.time;
            pathPoints.Add(transform.position);
        }

    }

    void OnMouseUp() {
        pathPoints.Add(transform.position);
    }

    public IEnumerator MoveToPosition(int targetIndex, float sampleRate) {
        if (targetIndex < pathPoints.Count) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / sampleRate;
                transform.position = Vector3.Lerp(transform.position, pathPoints[targetIndex], t);
                yield return null;
            }
            yield return MoveToPosition(targetIndex + 1, sampleRate);
        } else {
            ResetPosition();
            yield return MoveToPosition(1, sampleRate);
        }
    }

    public bool PathPointsPresent() {
        return pathPoints.Count > 0;
    }

    public void MatchSliderPosition(float sliderPercent) {
        int startingPathPointIndex = (int)(sliderPercent * (pathPoints.Count - 1)); // Casting automatically floors
        transform.position = pathPoints[startingPathPointIndex];
        float startingPathPointPercent = (float) startingPathPointIndex / ((float) pathPoints.Count - 1.0f);
        float t = (sliderPercent - startingPathPointPercent) / (1.0f / ((float) pathPoints.Count - 1.0f));
        transform.position = Vector3.Lerp(pathPoints[startingPathPointIndex], pathPoints[startingPathPointIndex+1], t);
    }

    public void ResetPosition() {
        transform.position = pathPoints[0];
    }

    public void StartPlaying() {
        ResetPosition();
        StartCoroutine(MoveToPosition(1, sampleRate));
    }

    public void StopPlaying() {
        StopAllCoroutines();
        ResetPosition();
    }

    //-------------------------------------------------------------------------------------

    public void DebugShowPathPoints() {
        for (int i = 0; i < pathPoints.Count; i++) {
            GameObject pathPointGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pathPointGameObject.transform.position = pathPoints[i];
            pathPointGameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            pathPointGameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void DebugAddPathPoint(Vector3 newPathPoint) {
        pathPoints.Add(newPathPoint);
    }

    public Vector3 DebugGetPathPoint(int pathPointIndex) {
        return pathPoints[pathPointIndex];
    }

}
