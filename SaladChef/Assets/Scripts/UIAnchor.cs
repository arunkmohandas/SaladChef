using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnchor : MonoBehaviour
{
    public Transform parentGameObject;
    private Canvas thisCanvas;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // parentGameObject=transform.parent.parent;
        cam=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
         this.GetComponent<RectTransform>().position=cam.WorldToScreenPoint(parentGameObject.position)-new Vector3(0,2,0);
    }
}
