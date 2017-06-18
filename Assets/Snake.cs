using UnityEngine;


public class Snake : MonoBehaviour {

	public float speed = 3f;
	public float rotationSpeed = 200f;
	float horizontal = 0f;

  
	// Update is called once per frame
	void Update () {
		horizontal = Input.GetAxisRaw ("Horizontal");
	}
	void FixedUpdate(){
		transform.Translate (Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
		//the - in the front of horizontal is for reverse control
		transform.Rotate (Vector3.forward*-horizontal*rotationSpeed*Time.fixedDeltaTime);

	}

	void OnTriggerEnter2D(Collider2D col){

		Debug.Log("collider: " + col.gameObject.name);
        if (col.tag == "food")
        {
            Tail.Grow(15);
            StartCoroutine(Tail.Flash());
            Food.Respawn();
        }
        if (col.gameObject.name == "Tail")
        {
            speed = 0f;
            rotationSpeed = 0f;
            GameObject.FindObjectOfType<GameManager>().EndGame();
        }

    }
    
}

