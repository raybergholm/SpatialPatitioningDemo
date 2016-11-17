using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{

	// MonoBehaviour chain of events: Awake() -> Start() -> runtime

	class Simulation : MonoBehaviour
	{
		enum Mode {NaiveArray, Quadtree, SpatialHash};
		
		int mode;
		const int ObjectCount = 100;
		const int PlaneWidth = 10;
		const int PlaneHeight = 10;
		
		// TODO: do I really need a SimObject wrapper for GameObject, or can I just stick with GameObject? the only extra things i need is a way to store the internal movement vector and a rollback state (might need to be dynamically calculated anyway)
		
		List<GameObject> items;
        GameObject cam;
		Rect gamePlane;
		//GamePlane gamePlane;
		
		GameObject obj;

        bool rebuildEveryTick;

        // Spatial partitioning objects
        // List<SimObject> items;			// for the naive array
        Quadtree quadtree;	                // quadtree
		SpatialHash spatialHash;		    // spatial hash
		
        
	
		void Start()
		{	
			gamePlane = new Rect(0,0, PlaneWidth, PlaneHeight);

            rebuildEveryTick = true;

            cam = GameObject.Find("MainCamera");
            cam.transform.LookAt(gamePlane.position); // TODO: check if the LookAt gives the right value. gamePlane won't change, so this should be good enough if the scaling is correct
			
			Pause(); // start sim paused
			ShowInitialMenu(); // initial menu sets the initial settings (object count, spatial partitioning, etc
			
			//InitializeObjects();
		}
		
		/* 
			Update vs FixedUpdate:
			http://gamedev.stackexchange.com/questions/93850/difference-between-update-method-and-fixedupdate-in-unity
			
			Update can vary based on FPS and is called once per frame. Time.deltaTime is the main way to track the time difference here.
			
			FixedUpdate attempts to be called at a regular rate:
				if the FPS is low (e.g. physics applies 50 times/sec but FPS is 20), it will be called multiple times to try to smooth out the differences in rates
				if the FPS is high (e.g. physics applies 50 times/sec but FPS is 60), it may be skipped between frames, also to smooth out the differences in rates
			In FixedUpdate, Time.deltaTime == Time.fixedDeltaTime
		*/
		
		/*
			unit * Time.deltaTime = unit per second
			
			so:
			transform.Translate(Vector3.up * 5) 					= move up 5 metres PER FRAME (FPS changes? movement rate also changes)
			transform.Translate(Vector3.up * 5 * Time.deltaTime) 	= move up 5 metres PER SECOND (FPS changes? movement is still 5 metres/second)
			
			however:
			in FixedUpdate, EVERYTHING is already going at the rate of units per second, so Time.deltaTime would not make sense when applying physics.
		*/
		
		void Update() // gameplay updates goes here! Also non-continuous physics like Physics.Raycast and Rigidbody.AddForce
		{
			// handle input
			if(Input.GetKey("pause"))
			{
				TogglePause();
			}
		}
		
		void FixedUpdate() // continuous physics goes here!
		{
			if(Time.timeScale > 0) // execute only if the sim is running! TODO: is this already handled by Unity?
			{
				List<GameObject> collisionCandidates;
				
				
				// enumerate over the object list and move then around
				StepObjects();
				
				// spatial partitioning time!
				switch(mode)
				{
					case (int)Mode.NaiveArray:
						// actually nothing needs to be 
						break;
					case (int)Mode.Quadtree:
                        if(rebuildEveryTick)
                        {
                            this.quadtree = BuildQuadtree();
                        }
                        else
                        {
                            UpdateQuadtree();
                        }
						//rebuildEveryTick ? quadtree = BuildQuadtree() : UpdateQuadtree(); // doesn't work? It's more compact than the if/else
						// TODO: broad phase with the quadtree
						break;
                    case (int)Mode.SpatialHash:
                        if (rebuildEveryTick)
                        {
                            this.spatialHash = BuildSpatialHash();
                        }
                        else
                        {
                            UpdateSpatialHash();
                        }
                        //rebuildEveryTick ? spatialHash = BuildSpatialHash() : UpdateSpatialHash();
						// TODO: broad phase with the spatial hash
						break;
					default:
						// this should never happen
						Debug.LogError("Invalid mode");
                        // TODO: maybe throw an exception to halt the program?
                        break;
				}
				
				// TODO: collisionCandidates contains propositions for narrow phase collision
				// maybe have it a collection of pairs?
			}
		}
		
		public void Pause()
		{
			Time.timeScale = 0; // pause time
		}
		
		public void Unpause()
		{
			Time.timeScale = 1.0F; // let time run at full speed
		}
		
		public void TogglePause()
		{
			if(Time.timeScale > 0)
			{
				Pause();
			}
			else
			{
				Unpause();
			}
		}
		
		private void InitializeObjects(int count = ObjectCount)
		{
            Vector3 randomVector;

            for (int i = 0; i < count; i++)
			{
				randomVector = GenerateRandomVector();
				
				obj = GameObject.CreatePrimitive(PrimitiveType.Sphere); //circle? how to slap this on the plane? or maybe let them overlap each other on the Z axis
				// TODO: some way to set the size to get the bounding volumes?
				// TODO: some way to tag the game object with a personal vector?
				obj.transform.position = GenerateRandomPosition(gamePlane);
				obj.AddComponent<MobileObjectBehaviour>();
				
				
				//myScript = this.gameObject.AddComponent<CLASS_WITH_MONO_BEHAVIOUR_SCRIPT>(); // one way to link a script (e.g. circleBehaviour) to the GameObject
				
				items.Add(obj);
			}
		}
		
		private void StepObjects()
		{
			foreach(GameObject item in items)
			{
				item.transform.Translate(new Vector3(0, 0, 0));
				// check for collision with gamePlane
				if(gamePlane.CheckColliding(item))
				{
					Vector3 intersectionVector = gamePlane.GetIntersectionVector(item);
					
					item.MoveByVector(intersectionVector);
				}
			}
		}
		
		private Quadtree BuildQuadtree()
		{
			Quadtree dataStruct = new Quadtree(gamePlane);
			
			foreach(GameObject item in items)
			{
				dataStruct.Insert(item);
			}
			
			return dataStruct;
		}
		
		private void UpdateQuadtree()
		{
			quadtree.Traverse(); 
			/* 
				TODO: 
				does C# have an equiv to:
				var callback = function(item, i){
					// do whatever
				}
				quadtree.traverse(callback);
			*/
		}
		
		private SpatialHash BuildSpatialHash()
		{
            const int bucketSize = 64;
			SpatialHash dataStruct = new SpatialHash(bucketSize);
			foreach(GameObject item in items)
			{
				dataStruct.Insert(item.GetComponent<MobileObjectBehaviour>().aabbBV, item);
			}
			return dataStruct;
		}
		
		private void UpdateSpatialHash()
		{
			
		}
		
		public Vector3 GenerateRandomPosition(Rect area)
		{
			return new Vector3(UnityEngine.Random.Range(area.xMin, area.xMax), UnityEngine.Random.Range(area.yMin, area.yMax), 0);
		}
		
		public Vector3 GenerateRandomVector()
		{
			// keep all the generated vectors within a circle, having x = [a, b], y = [a, b] makes the vectors more biased towards a faster diagonal magnitude which will look weird
			
			float direction = UnityEngine.Random.Range(0, 359 * Mathf.Deg2Rad);	// 0 to 359 degrees -> 0 to almost 2pi radians
			float magnitude = UnityEngine.Random.Range(1, 10);		// every object should be in motion

			float xComponent = magnitude * Mathf.Cos(direction);
			float yComponent = magnitude * Mathf.Sin(direction);
			
			return new Vector2(xComponent, yComponent);
		}
		
        public void ShowInitialMenu()
        {

        }
    }
	
    /*
	class SimObject
	{
		private GameObject obj;
		private Vector3 oldPosition; // current position is obj.transform.position
		private Vector3 movementVector;
		
		public SimObject(string objectId, Vector3 position, Vector3 movementVector)
		{
			obj = GameObject.CreatePrimitive(PrimitiveType.Sphere); // some circle primitive?
			
			obj.transform.position = position;
			this.movementVector = movementVector;
			
		}
		
		public void MoveByInternalVector()
		{
			oldPosition = obj.transform.position; // TODO: is this a ref copy?
			obj.transform.position = position + movementVector * Time.deltaTime;
		}
		
		public void MoveByVector(Vector3 moveVector)
		{
			oldPosition = position; // TODO: is this a ref copy?
			position = position + moveVector;
		}
		
		public void RollbackPosition(Vector2 newPosition)
		{
			position = newPosition;
		}
		
		public bool CheckCollision(SimObject other) // circle to circle collision
		{
			float squaredHypotenuse = ((this.position.x - other.position.x) * (this.position.x - other.position.x)) + ((this.position.x - other.position.y) * (this.position.x - other.position.y));
			return squaredHypotenuse < this.radius + other.radius;
		}

        // User-defined conversion SimObject -> GameObject
        public static implicit operator GameObject(SimObject simObj)
        {
            return simObj.obj;
        }
		
		// User-defined conversion SimObject -> Vector3
        public static implicit operator Vector3(SimObject simObj)
        {
            return simObj.obj.transform.position;
        }
		
		// User-defined conversion SimObject -> Rect
        public static implicit operator Rect(SimObject simObj)
        {
            // TODO: some way to get the AABB of this object
        }

    }
    */
}