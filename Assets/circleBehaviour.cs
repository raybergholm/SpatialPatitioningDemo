using UnityEngine;
using System.Collections;
 
public class circleBehaviour : MonoBehaviour {
 
	public float speed = 3;
	private float xSpeed;
	private float ySpeed;
 
	// Use this for initialization
	void Start () {
		float angle = Random.Range(0, 2 * Mathf.PI);
		xSpeed = speed * Mathf.Cos(angle) / 100;
		ySpeed = speed * Mathf.Sin(angle) / 100;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(xSpeed, ySpeed, 0);
		if(transform.position.x < -2.4){
			transform.position = new Vector3(-2.4F, transform.position.y, 0);
			xSpeed *= -1;
		}
		if(transform.position.x > 2.4){
			transform.position = new Vector3(2.4F, transform.position.y, 0);
			xSpeed *= -1;
		}
		if(transform.position.y < -2.4){
			transform.position = new Vector3(transform.position.x, -2.4F, 0);
			ySpeed *= -1;
		}
		if(transform.position.y > 2.4){
			transform.position = new Vector3(transform.position.x, 2.4F, 0);
			ySpeed *= -1;
		}
	}
}