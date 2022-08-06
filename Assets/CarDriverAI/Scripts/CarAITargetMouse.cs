using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAITargetMouse : MonoBehaviour {

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float verticalOffsetFromMouse = 1f;

    private bool isFollowing = false;

    private void LateUpdate() {
        if (isFollowing) {
            targetTransform.position = Mouse3D.GetMouseWorldPosition() + new Vector3(0,verticalOffsetFromMouse,0);
        }

        if (Input.GetMouseButtonDown(0)) {
            isFollowing = !isFollowing;
        }
    }

}
