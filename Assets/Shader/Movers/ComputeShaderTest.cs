using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    int moverNum = 16 * 100;
    Mover[] movers;

    private void Start()
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }
        movers = new Mover[moverNum];

        for (int i = 0; i < moverNum; i++)
        {
            movers[i].pos = Random.insideUnitCircle * 1080 + new Vector2(renderTexture.width / 2, renderTexture.height / 2);//Random.insideUnitCircle * 10 + new Vector2(renderTexture.width/2, renderTexture.height/2);
            if(i%3 == 0)
                movers[i].angle = Mathf.Deg2Rad * Random.Range(100, 80);

            if (i % 3 == 1)
                movers[i].angle = Mathf.Deg2Rad * Random.Range(100+120, 80 +120);

            if (i % 3 == 2)
                movers[i].angle = Mathf.Deg2Rad * Random.Range(100 + 240, 80 + 240);

        }
    }
    struct Mover
    {
        public Vector2 pos;
        public float angle;
    };


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(renderTexture == null)
        {
            renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }


        int totalSize = sizeof(float) * 3;
        ComputeBuffer buffer = new ComputeBuffer(moverNum, totalSize);
        buffer.SetData(movers);
        computeShader.SetBuffer(0,"movers",buffer);
        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetFloat("Resolution", renderTexture.width);
        computeShader.SetFloat("width", renderTexture.width);
        computeShader.SetFloat("height", renderTexture.height);
        computeShader.SetFloat("moveSpeed", 50);
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.SetInt("moverNum", moverNum);
        computeShader.Dispatch(0, moverNum / 16, 1, 1);

        //secondFunc


        int kernel = computeShader.FindKernel("Trail");
        computeShader.SetTexture(kernel, "TrailPro", renderTexture);
        computeShader.SetTexture(kernel, "Result", renderTexture);
        computeShader.SetFloat("dissSpeed", .1f);
        computeShader.SetFloat("diffSpeed", 100f);
        computeShader.Dispatch(kernel, renderTexture.width / 8
            , renderTexture.height / 8, 1);

        buffer.GetData(movers);
        
        buffer.Dispose();

        Graphics.Blit(renderTexture, destination);
    }
}
