using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour
{
    public static CameraScaling instance;

    public Camera m_OrthographicCamera;
    public float sceneWidth = 10;

    private float defaultOrthoSize;

    private void Awake()
    {
        instance = this;
        defaultOrthoSize = m_OrthographicCamera.orthographicSize;
    }

    private void Start()
    {
        UpdateCameraScale();
    }

    public void UpdateCameraScale()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        m_OrthographicCamera.orthographicSize = desiredHalfHeight;
        m_OrthographicCamera.transform.position = new Vector3(0, desiredHalfHeight - defaultOrthoSize, -10);
    }

}
