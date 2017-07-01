/*
 * BrainSnake code by
 * Pietro Romeo & Marc van Almkerk
 * June 2017
 */

//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[RequireComponent (typeof(LineRenderer))]
[RequireComponent (typeof(EdgeCollider2D))]

public class Tail : MonoBehaviour {
	static LineRenderer line;
	static EdgeCollider2D col;

	static List<Vector2> points;
	public float pointSpacing =0.1f;
	public Transform snake;
    public GameManager GM;
    private static Vector3 snakepos;
    private int length;
    private static Gradient grad;


    // Use this for initialization
    void Start () {
		line = GetComponent<LineRenderer> ();
		col = GetComponent <EdgeCollider2D> ();
		points = new List<Vector2>();
        grad = line.colorGradient;

        length = 15;
        SetPoint(new Vector3(0, 0, 0));
    }



	// Update is called once per frame
	void Update () {
        snakepos = snake.position;

        if (NetworkServer.active)
        {
            if (Vector2.Distance(points.Last(), snakepos) > pointSpacing)
                 GM.RpcSetTailPoint(snakepos);
        }
    }

    public void SetPoint(Vector3 pos) {
        if (points.Count > 6)
            col.points = points.GetRange(5,points.Count-5).ToArray<Vector2>();

        if (points.Count == length)
        {
            points.RemoveAt(0);
            points.Add(pos);
        }
        else points.Add(pos);

        line.numPositions = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    public void reset()
    {
        points.Clear();
        SetPoint(new Vector3(0, 0, 0));
        length = 15;
    }

    public void Grow(int size)
    {
        length += size;
    }

    public static IEnumerator<WaitForSeconds> Flash()
    {
        Color w = Color.white;

        for (int i = 0; i < 3; i++)
        {
            line.colorGradient = grad;
            yield return new WaitForSeconds(1 / 5f);
            line.startColor = w;
            line.endColor = w;
            yield return new WaitForSeconds(1 / 5f);
            line.colorGradient = grad;
            yield return new WaitForSeconds(1 / 5f);
            line.startColor = w;
            line.endColor = w;
            yield return new WaitForSeconds(1 / 5f);
            line.colorGradient = grad;

        }

    }

}

