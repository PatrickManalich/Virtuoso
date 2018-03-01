using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandTriggerScript : MonoBehaviour {


    private OvrAvatar ovrAvatarScript;
    private GameObject hoveringBand;
    private int numOfBandsAlive;

    private void Awake() {
        ovrAvatarScript = transform.parent.parent.GetComponent<OvrAvatar>();
        hoveringBand = null;
        numOfBandsAlive = 0;
        StartCoroutine(SetPosition());
    }

    private IEnumerator SetPosition() {
        if (!ovrAvatarScript.GetHandTransform(OvrAvatar.HandType.Right, OvrAvatar.HandJoint.IndexTip)) {
            yield return new WaitForSeconds(1f);
            yield return SetPosition();
        } else {
            transform.position = ovrAvatarScript.GetHandTransform(OvrAvatar.HandType.Right, OvrAvatar.HandJoint.IndexTip).position;
            yield return null;
        }
    }

    
    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.tag == "Band") {
            hoveringBand = otherCollider.gameObject;
            hoveringBand.GetComponent<BandScript>().GiveLife();
            numOfBandsAlive++;
        }
    }

    private void OnTriggerStay(Collider otherCollider) {
        if (otherCollider.tag == "Band" && numOfBandsAlive > 0)
            hoveringBand.GetComponent<BandScript>().IncreaseLifetime();
    }

    private void OnTriggerExit(Collider otherCollider) {
        if (otherCollider.tag == "Band") {
            numOfBandsAlive--;
            if(numOfBandsAlive == 0)
                hoveringBand = null;
        }
    }

    public void BandGrabbed() {
        if (hoveringBand)
            hoveringBand.GetComponent<BandScript>().Toggle();
    }


}
