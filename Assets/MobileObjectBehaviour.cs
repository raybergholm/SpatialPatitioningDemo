using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{
	public class MobileObjectBehaviour : MonoBehaviour
	{
        // TODO: WTF was I doing here anyway?

		//private Vector3 oldPosition; // current position is obj.transform.position
		//private Vector3 movementVector;
	    
  //      public Circle circleBV
  //      {
  //          get { return this.circleBV; }
  //          set { this.circleBV = value; }
  //      }

  //      public Rect aabbBV
  //      {
  //          get { return this.aabbBV; }
  //          set { this.aabbBV = value; }
  //      }

  //      /*
  //      public Obb obbBV
  //      {
  //          get { return this.obbBV; }
  //          set { this.obbBV = value; }
  //      }
  //      */

  //      public void setBV(Vector3 pos)
  //      {
  //          aabbBV = new Rect();

  //          circleBV = new Circle(pos, 10);
  //      }
		
		//public void setBV(Rect bounds)
		//{
		//	aabbBV = bounds;
			
		//	circleBV = new Circle((Vector3)bounds.center, bounds.center.x - bounds.xMin);
		//}
		
		//public void setBV(Circle bounds)
		//{
		//	circleBV = bounds;
			
		//	aabbBV = (Rect)bounds;
		//}

  //      public MobileObjectBehaviour()
  //      {
  //          setBV(gameObject.transform.position);
  //      }

  //      void Start()
		//{
		
		//}
		
		//void Update()
		//{
		//	// alternative: if we have a frictionless medium, a one-off rigidbody.AddForce might work for movement?
		//}
		
		//void FixedUpdate()
		//{
		//	oldPosition = gameObject.transform.position;	// TODO: check if this assignment leads to a copy or a ref
		//	gameObject.transform.Translate(movementVector);
		//}

  //      public void InitWithRandomValues()
  //      {
  //          // keep all the generated vectors within a circle, having x = [a, b], y = [a, b] makes the vectors more biased towards a faster diagonal magnitude which will look weird

  //          float direction = UnityEngine.Random.Range(0, 359 * Mathf.Deg2Rad); // 0 to 359 degrees -> 0 to almost 2pi radians
  //          float magnitude = UnityEngine.Random.Range(1, 10);      // every object should be in motion

  //          float xComponent = magnitude * Mathf.Cos(direction);
  //          float yComponent = magnitude * Mathf.Sin(direction);

  //          movementVector = new Vector3(xComponent, yComponent, 0);
  //      }
	}
	
}