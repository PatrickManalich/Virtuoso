using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandTriggerScript : MonoBehaviour {

    private OvrAvatar ovrAvatarScript;

    private void Awake() {
        ovrAvatarScript = transform.parent.parent.GetComponent<OvrAvatar>();
    }

    private void Update() {
        Transform jointTipTransform = ovrAvatarScript.GetHandTransform(OvrAvatar.HandType.Right, OvrAvatar.HandJoint.IndexTip);
        if (jointTipTransform)
            transform.position = jointTipTransform.position;
    }
    
}
