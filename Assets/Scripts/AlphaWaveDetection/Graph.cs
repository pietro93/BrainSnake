using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public static float YMin = -1, YMax = +1;

    public const int MAX_HISTORY = 1024;
    public const int MAX_CHANNELS = 5;

    public static Channel[] channel = new Channel[MAX_CHANNELS];

    static Graph()
    {
        Graph.channel[0] = new Channel(Color.black);
        Graph.channel[1] = new Channel(Color.red);
        Graph.channel[2] = new Channel(Color.green);
        Graph.channel[3] = new Channel(Color.yellow);
        Graph.channel[4] = new Channel(Color.blue);
    }

}