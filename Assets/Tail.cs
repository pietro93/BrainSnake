//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(LineRenderer))]
[RequireComponent (typeof(EdgeCollider2D))]

public class Tail : MonoBehaviour {
	static LineRenderer line;
	EdgeCollider2D col;

	static List<Vector2> points;
	public float pointSpacing =.1f;
	public Transform snake;

    private static int length;
    


	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
		col = GetComponent <EdgeCollider2D> ();
		points = new List<Vector2>();
        length = 1;
        points.Add(snake.position);
        Grow(4);
    }



	// Update is called once per frame
	void Update () {
        if (Vector3.Distance (points.Last(), snake.position )> pointSpacing)
			SetPoint ();
        if (points.Count == length)
            points.Remove(points.First());

    }

    void SetPoint() {
        if (points.Count > 1)
        {
            col.points = points.ToArray<Vector2>();
            
        }

        points.Add(snake.position);

        line.numPositions = points.Count;
        
        for (int i = 0; i < length; i++)
        {
            line.SetPosition(i, points.ElementAt(i));
        }
        line.SetPosition(points.Count - 1, snake.position);
    }

    public static void Grow(int size)
    {
        for (int i = 0; i < size; i++)
            points.Add(points.Last());
        length += size;
        print("snake length: " + length);
    }

    public static IEnumerator<WaitForSeconds> Flash()
    {
        Gradient g = line.colorGradient;
        Color c1 = Color.green;
        print(g);
        for (int i = 0; i < 3; i++)
        {
            line.startColor = c1;
            line.endColor = c1;
            yield return new WaitForSeconds(1/5f);
            line.colorGradient = g;
            yield return new WaitForSeconds(1/5f);
        }
    }

}

