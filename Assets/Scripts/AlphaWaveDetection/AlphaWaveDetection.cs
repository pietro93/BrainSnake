using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWaveDetection : MonoBehaviour
{

    static public bool Display = true;
    static public bool AlphaDetected = false;

    private LSLcontroller modifier;
    private float smoothVal = 0.0f;
    private float rawVal = 0.0f;
    private int sampleCount = 0;

    public float smoothing = 0.9f;
    public float threshold = 1.7f;
    public int sampleOffset = 7;

    // Use this for initialization
    void Start()
    {
        modifier = GetComponent<LSLcontroller>();

        //set graph dimensions
        Graph.YMin = 0f;
        Graph.YMax = 3f;

        Graph.channel[0].isActive = true; //axis
        Graph.channel[1].isActive = true; //raw data
        Graph.channel[1].line = false;
        Graph.channel[2].isActive = true; //smoothed data
        Graph.channel[3].isActive = true; //smoothed data over threshold
        Graph.channel[4].isActive = true; //threshold
    }

    // Update is called once per frame
    void Update()
    {

        if (modifier.value >= 0)
        {
            //update raw data and smoothed data (IIR)
            rawVal = modifier.value;
            smoothVal = smoothVal * smoothing + rawVal * (1 - smoothing);

            if (AlphaWaveDetection.AlphaDetected)
            {
                //count raw values below threshold
                if (rawVal < threshold) sampleCount++;
                else sampleCount = 0;

                //Too much values below threshold or smoothval below threshold --> no alpha waves
                if (sampleCount >= sampleOffset || smoothVal < threshold)
                {
                    smoothVal = rawVal;
                    AlphaWaveDetection.AlphaDetected = false;
                    sampleCount = 0;
                }
            }
            else
            {
                //count smooth values above threshold
                if (smoothVal >= threshold) sampleCount++;
                else sampleCount = 0;

                //detect alpha waves
                if (sampleCount >= sampleOffset)
                {
                    AlphaWaveDetection.AlphaDetected = true;
                    sampleCount = 0;
                }
            }

            modifier.value = -1;
        }

        if (AlphaWaveDetection.Display) SendDataToGraph();
    }

    public void SendDataToGraph()
    {
        //UPDATE GRAPH
        Graph.channel[0].Feed(0); //axis line (0)
        Graph.channel[1].Feed(rawVal); //feed raw data
        if (smoothVal > threshold) //update smoothed data
        {
            //if over threshold make line green...
            Graph.channel[2].Feed(smoothVal);
            Graph.channel[3].Feed(threshold);
        }
        else
        {
            //otherwise keep it yellow
            Graph.channel[2].Feed(smoothVal);
            Graph.channel[3].Feed(smoothVal);
        }
        Graph.channel[4].Feed(threshold); //threshold line
    }
}
