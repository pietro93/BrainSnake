/*
 * Code by Pietro Romeo
 * June 2017
 */

//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(LineRenderer))]
[RequireComponent (typeof(EdgeCollider2D))]

public class Tail : MonoBehaviour {
	static LineRenderer line;
	static EdgeCollider2D col;

	static List<Vector2> points;
	public float pointSpacing =0.1f;
	public Transform snake;
    private static Vector3 snakepos;
    private static int length;
    private static Gradient grad;




    // Use this for initialization
    void Start () {
		line = GetComponent<LineRenderer> ();
		col = GetComponent <EdgeCollider2D> ();
		points = new List<Vector2>();
        grad = line.colorGradient;
        length = 1;
        points.Add(snake.position);
        Grow(4);
    }



	// Update is called once per frame
	void Update () {
        snakepos = snake.position;
        
        if (Vector3.Distance (points.Last(), snakepos )> pointSpacing)
			SetPoint ();
        if (points.Count == length)
            points.Remove(points.First());
         
        if (length < 30) // dirty hack to avoid the snake dying in the beginning if it hits a wall
        {
            col.enabled = false;
        }
        else
            col.enabled = true;

    }

    void SetPoint() {
        if (points.Count > 1) { 
            col.enabled = false;
            col.points = points.GetRange(0, points.Count() - 1).ToArray();
        }

        points.Add(snakepos); 
        line.numPositions = points.Count;
        
        for (int i = 0; i < length; i++)
        {
            line.SetPosition(i, points.ElementAt(i));
        }
        line.SetPosition(length - 1, snakepos);
        col.enabled = true;
    }

    public void reset()
    {
        points.Clear();
        length = 1;
        points.Add(snake.position);
        Update();
        Grow(4);
    }

    public static void Grow(int size)
    {
        for (int i = 0; i < size; i++)
        {
            points.Add(snakepos);
            length++;

        }

        Debug.Log("snake length: " + length);
        
    }

    public static IEnumerator<WaitForSeconds> Flash()
    {
        //UPDATE fixed flickering 22.06
        Color w = Color.white;

        for (int i = 0; i < 3; i++)
        {
            line.colorGradient = grad;
            yield return new WaitForSeconds(1/5f);
            line.startColor = w;
            line.endColor = w;
            yield return new WaitForSeconds(1/5f);
            line.colorGradient = grad;
            yield return new WaitForSeconds(1/5f);
            line.startColor = w;
            line.endColor = w;
            yield return new WaitForSeconds(1/5f);
            line.colorGradient = grad;

        }

       
        
    }

}

