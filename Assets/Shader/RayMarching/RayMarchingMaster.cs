using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMarchingMaster : MonoBehaviour
{
    public ComputeShader computeShader;
    private RenderTexture renderTexture;
    private Camera _camera;

    public float power;
    public float smoothMinVal;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }
     

    }
   


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }



        computeShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        computeShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetInt("Resolution", renderTexture.width);
        computeShader.SetInt("width", renderTexture.width);
        computeShader.SetInt("height", renderTexture.height);
        computeShader.SetFloat("Time", Time.time);
        computeShader.SetFloat("power", power);
        power += 0.2f * Time.deltaTime;
        computeShader.SetFloat("smoothMinVal",smoothMinVal);
       
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);


        Graphics.Blit(renderTexture, destination);
    }
}
