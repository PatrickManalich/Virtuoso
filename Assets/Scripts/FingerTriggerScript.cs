using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTriggerScript : MonoBehaviour {

    private OvrAvatar ovrAvatarScript;  // The OVRAvatarScript component of LocalAvatar

    private void Awake() {
        ovrAvatarScript = transform.parent.parent.GetComponent<OvrAvatar>();
    }

    private void Update() {
            // Update the position of the finger trigger based on the position of the index tip of its parent's hand
        Transform indexTipTransform = ovrAvatarScript.GetHandTransform(OvrAvatar.HandType.Right, OvrAvatar.HandJoint.IndexTip);
        if (indexTipTransform)
            transform.position = indexTipTransform.position;
    }
}
