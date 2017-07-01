using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccuracyCtrl : MonoBehaviour {

    public Text command;
    public AudioSource bell;
    private string filePath;
    public string participant = "1";
    StreamWriter writer;
    private static string delimiter = ",";

    private DateTime startTime;
    private double EyesCloseDelay;
    private double EyesOpenDelay;
    private int FP,TP,TN,FN = 0;

    public bool saved;
    public bool alpha;

    private bool alphaPrev = false;
    private bool detected = false;
    private bool measuringClosed = false;
    private bool measuringOpen = false;
    private float StartTime;
    private float iterations = 1;

    // Use this for initialization
    void Start () {
        // Prepare data saving, MAKE CSV FILE
        filePath = getPath();
        writer = new StreamWriter(filePath);
        writer.WriteLine("CloseEyeDelay" + delimiter + "OpenEyesDelay" + delimiter + "True positives" + delimiter + "False positives" + delimiter + "True negatives" + delimiter + "False negatives" + Environment.NewLine);
        writer.Flush();

        command.CrossFadeAlpha(0, 0, false);
	}

    IEnumerator Sequence()
    {
        //COMMAND 1
        command.text = "Close your eyes when you hear the sound";
        command.CrossFadeAlpha(1, 0.5f, false);
        bell.Play();
        yield return new WaitForSeconds(7f);
        command.CrossFadeAlpha(0, 0.5f, false);
        yield return new WaitForSeconds(0.5f);

        //COMMAND 2
        command.text = "Open your eyes when you hear the sound for the second time";
        command.CrossFadeAlpha(1, 0.5f, false);
        bell.Play();
        yield return new WaitForSeconds(7f);
        command.CrossFadeAlpha(0, 0.5f, false);
        yield return new WaitForSeconds(0.5f);

        //COMMAND 3
        command.text = "Okay, let's get started!";
        command.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(5f);
        command.CrossFadeAlpha(0, 0.5f, false);
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds((UnityEngine.Random.value * 3) + 2);
        for (iterations = 1; iterations <= 6; iterations++)
        {
            //EYES --> close
            bell.Play();
            measuringClosed = true;
            measuringOpen = false;
            startTime = DateTime.Now;
            alphaPrev = false;
            detected = false;

            //EYES --> open
            yield return new WaitForSeconds((UnityEngine.Random.value * 3) + 2);
            bell.Play();
            measuringClosed = false;
            measuringOpen = true;
            startTime = DateTime.Now;
            alphaPrev = true;
            detected = false;

            yield return new WaitForSeconds((UnityEngine.Random.value * 3) + 2);
            Write();
            FP = TP = TN = FN = 0;
            measuringClosed = false;
            measuringOpen = false;
        }

        command.text = "Accuracy test is over, thank you!";
        command.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(30f);

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            StartCoroutine("Sequence");
        }
        else if(Input.GetKeyDown("x"))
        {
            SceneManager.LoadScene("MainLevel");
        }

        if (Input.GetKey("t"))
        {AlphaWaveDetection.AlphaDetected = true;}
        else
        {AlphaWaveDetection.AlphaDetected = false;}

        if (measuringClosed)
        {
            if(AlphaWaveDetection.AlphaDetected && !alphaPrev)
            {
                if (!detected)
                {
                    EyesCloseDelay = (DateTime.Now - startTime).TotalSeconds;
                    detected = true;
                }
                alphaPrev = true;
            }
            else if(alphaPrev)
            {
                if (AlphaWaveDetection.AlphaDetected) TP++;
                else FP++;
            }
        }
        else if(measuringOpen)
        {
            if (!AlphaWaveDetection.AlphaDetected && alphaPrev)
            {
                if (!detected)
                {
                    EyesOpenDelay = (DateTime.Now - startTime).TotalSeconds;
                    detected = true;
                }
                alphaPrev = false;
            }
            else if (!alphaPrev)
            {
                if (!AlphaWaveDetection.AlphaDetected) TN++;
                else FN++;
            }
        }
    }

    public string getPath()
    {
        if (!File.Exists(participant + "a.csv"))
        {
            return Application.dataPath + "/CSV/" + participant + "a.csv";
        }
        else if (File.Exists(participant + "a.csv") && !File.Exists(participant + "b.csv"))
        {
            return Application.dataPath + "/CSV/" + participant + "b.csv";
        }
        else if (File.Exists(participant + "a.csv") && File.Exists(participant + "b.csv"))
        {
            return Application.dataPath + "/CSV/" + participant + "c.csv";
        }
        else
        {
            Debug.Log("Already recorded three trials for participant " + participant);
            return Application.dataPath + "/CSV/" + participant + "error.csv";
        }

    }

    void Write()
    {
        writer.WriteLine(EyesCloseDelay.ToString() + delimiter + EyesOpenDelay.ToString() + delimiter + TP.ToString() + delimiter + FP.ToString() + delimiter + TN.ToString() + delimiter + FN.ToString());
        writer.Flush();
    }

    void Save()
    {
        writer.Close();
        saved = true;
    }

}
