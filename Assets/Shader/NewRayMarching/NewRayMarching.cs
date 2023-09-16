using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRayMarching : MonoBehaviour
{
    public int dirSign = 1;
    public ComputeShader computeShader;
    public Light light;
    private RenderTexture renderTexture;
    private Camera _camera;

    public float power;
    public float smoothMinVal;

    

    public Vector3 color1Power;
    public Vector3 color2Power;
    //
    float dark = 30;
    public Slider darkSlider;

    float beforeRotateX;
    public Slider beforeRotXSlider;

    bool animateX = false;
    public Toggle animateXTog;

    float beforeRotateY;
    public Slider beforeRotYSlider;

    public bool animateY = false;
    public Toggle animateYTog;

    float beforeRotateZ;
    public Slider beforeRotZSlider;

    bool animateZ = false;
    public Toggle animateZTog;

    float afterRotateX = 0;
    public Slider afterRotXSlider;

    float afterRotateY = 0;
    public Slider afterRotYSlider;

    float afterRotateZ = 0;
    public Slider afterRotZSlider;

    int Iterations = 15;
    public Slider iterationSlider;

    float minDist = 0.005f; // = 0.005f;
    public Slider detailSlider;

    int maxStepCount = 150; // = 150;
    public Slider stepSlider;


    public Slider red;
    public Slider green;
    public Slider blue;

    public Slider cameraZSlider;
    float camStartZ;
    private void Awake()
    {
        camStartZ = transform.localPosition.z;
       
        _camera = GetComponent<Camera>();
    }

    void initVariables()
    {
        dark = darkSlider.value;
        beforeRotateX = beforeRotXSlider.value;
        animateX = animateXTog.isOn;
        beforeRotateY = beforeRotYSlider.value;
        animateY = animateYTog.isOn;
        beforeRotateZ = beforeRotZSlider.value;
        animateZ = animateZTog.isOn;

        afterRotateX = afterRotXSlider.value;
        afterRotateY = afterRotYSlider.value;
        afterRotateZ = afterRotZSlider.value;

        Iterations = (int)iterationSlider.value;
        minDist = detailSlider.value;
        maxStepCount = (int)stepSlider.value;

        color1Power.x = red.value;
        color1Power.y = green.value;
        color1Power.z = blue.value;

        Vector3 localPos = transform.localPosition;
        localPos.z = camStartZ + cameraZSlider.value;
        transform.localPosition = localPos;
        
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
        initVariables();
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
        //computeShader.SetFloat("Time", Time.time);
        computeShader.SetFloat("power", power);
        computeShader.SetFloat("dark", dark);
        computeShader.SetVector("LightDirection", light.transform.forward);
        computeShader.SetVector("color1Power", color1Power);
        computeShader.SetVector("color2Power", color2Power);
        power += 0.2f * Time.deltaTime * dirSign;
        computeShader.SetFloat("smoothMinVal", smoothMinVal);
        //
        computeShader.SetFloat("afterRotateX", afterRotateX);
        computeShader.SetFloat("afterRotateY", afterRotateY);
        computeShader.SetFloat("afterRotateZ", afterRotateZ);

        //before Rotate -------------

        
        if (animateX)
        {
            beforeRotateX += Time.deltaTime * 0.2f;
            if (beforeRotateX > Mathf.PI * 2)
                beforeRotateX -= Mathf.PI * 2;

            beforeRotXSlider.value = beforeRotateX;
        }
        computeShader.SetFloat("beforeRotateX", beforeRotateX);

        if (animateY)
        {
            beforeRotateY += Time.deltaTime * 0.2f;
            if (beforeRotateY > Mathf.PI * 2)
                beforeRotateY -= Mathf.PI * 2;
            beforeRotYSlider.value = beforeRotateY;
        }
        computeShader.SetFloat("beforeRotateY", beforeRotateY);

        if (animateZ)
        {
            beforeRotateZ += Time.deltaTime * 0.2f;
            if (beforeRotateZ > Mathf.PI * 2)
                beforeRotateZ -= Mathf.PI * 2;
            beforeRotZSlider.value = beforeRotateZ;
        }
        computeShader.SetFloat("beforeRotateZ", beforeRotateZ);


        //before Rotate -------------


        computeShader.SetInt("Iterations", Iterations);
        if(minDist < 0)
        {
            minDist = 0;
        }
        computeShader.SetFloat("minDist", minDist/1000 + 0.001f);
        computeShader.SetFloat("maxStepCount", maxStepCount);

        //

        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);


        Graphics.Blit(renderTexture, destination);
    }
}
