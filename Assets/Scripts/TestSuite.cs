using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SpatialPartitioning.Quadtree;
using SpatialPartitioning.SpatialHash;

public class TestSuite : MonoBehaviour
{

    public enum Mode { NaiveArray, Quadtree, SpatialHash };

    int mode;
    const int PlaneWidth = 100;
    const int PlaneHeight = 100;
	float gridX;
	float gridY;
	float gridStep = 10;
        
        
    List<GameObject> objectList;
    GameObject cam;
    GameObject gamePlane;
    Bounds gamePlaneBounds;
    //GamePlane gamePlane;

    GameObject obj;

    bool rebuildEveryTick;

    // Spatial partitioning objects
    // List<SimObject> items;			// for the naive array
    Quadtree quadtree;                  // quadtree
    SpatialHash spatialHash;            // spatial hash

    void Start()
    {
        gamePlane = GameObject.Find("GamePlane");
        //gamePlane.transform.localScale = new Vector3(PlaneWidth, PlaneHeight, 0);
        gamePlaneBounds = new Bounds(Vector3.zero, new Vector3(PlaneWidth / 2, PlaneHeight / 2, 0));
            
		gridX = gamePlaneBounds.center.x - gamePlaneBounds.extents.x;
		gridY = gamePlaneBounds.center.y - gamePlaneBounds.extents.y;
			
		objectList = new List<GameObject>();
		quadtree = new Quadtree(gamePlaneBounds);
		spatialHash = new SpatialHash(20);
			
        rebuildEveryTick = true;

        cam = GameObject.Find("Main Camera");
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
        // handle test input
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Test1();
        }
		else if (Input.GetKey(KeyCode.Alpha2))
        {
            Test2();
        }
		else if (Input.GetKey(KeyCode.Alpha3))
        {
            Test3();
        }
		else if (Input.GetKey(KeyCode.Alpha4))
        {
            Test4();
        }
			
		if(Input.GetKey(KeyCode.Z))
		{
			StepTestRemove();
		}
		else if (Input.GetKey(KeyCode.X))
        {
            StepQuadtreeTestAdd();
        }
		else if (Input.GetKey(KeyCode.C))
        {
			StepQuadtreeGridTestAdd();    
        }
		else if (Input.GetKey(KeyCode.V))
        {
            StepSpatialHashTestAdd();
        }
		else if (Input.GetKey(KeyCode.B))
        {
			StepSpatialHashGridTestAdd();    
        }
			
			
			
		// camera forward/backward movement
        if(Input.GetKey(KeyCode.W))
        {
            cam.transform.position += Vector3.forward;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            cam.transform.position += Vector3.back;
        }
			
		// camera left/right movement
        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += Vector3.right;
        }
			
		// camera up/down movement
        if (Input.GetKey(KeyCode.Space))
        {
            cam.transform.position += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            cam.transform.position += Vector3.down;
        }

        cam.transform.LookAt(gamePlane.transform.position); // TODO: check if the LookAt gives the right value. gamePlane won't change, so this should be good enough if the scaling is correct

        if (Input.GetKey(KeyCode.P))
		{
			TogglePause();
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
        if (Time.timeScale > 0)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    private void UpdateQuadtree()
    {
        //quadtree.Traverse();
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
        foreach (GameObject item in objectList)
        {
            //dataStruct.Insert(item.GetComponent<MobileObjectBehaviour>().aabbBV, item);
        }
        return dataStruct;
    }

    private void UpdateSpatialHash()
    {

    }

    public Vector3 GenerateRandomPosition(Bounds bounds)
    {
        return new Vector3(
			UnityEngine.Random.Range(bounds.center.x - bounds.extents.x + 1, bounds.center.x + bounds.extents.x - 1), 
			UnityEngine.Random.Range(bounds.center.y - bounds.extents.y + 1, bounds.center.y + bounds.extents.y - 1), 
			bounds.center.z);
    }
		
	public Vector3 GenerateGridPosition(Bounds bounds)
	{
		Vector3 vec = new Vector3(gridX, gridY, 0);
			
		gridX += gridStep;
		if(gridX >= bounds.center.x + bounds.extents.x)
		{
			gridX = bounds.center.x - bounds.extents.x;
			gridY += gridStep;
		}
			
		return vec;
	}

    public void ShowInitialMenu()
    {
			
    }
		
	public void InitialiseTestObjects(int count)
	{
        GameObject obj;

        // equal grid format
        for (int i = 0; i < count; i++)
        {
			StepTestAdd();
        }
	}

    private void StepTestAdd()
    {

    }

    private void InitializeGridObjects() // grid
    {
        Bounds plane = gamePlaneBounds;
        float xMin = Mathf.Floor(plane.center.x - plane.extents.x);
        float xMax = Mathf.Floor(plane.center.x + plane.extents.x);
        float yMin = Mathf.Floor(plane.center.y - plane.extents.y);
        float yMax = Mathf.Floor(plane.center.y + plane.extents.y);
        int xSpacing = 64;
        int ySpacing = 64;

        for(var x = xMin; x < xMax; x += xSpacing)
        {
			for(var y = yMin; y < yMax; y += ySpacing)
			{
				obj = GameObject.CreatePrimitive(PrimitiveType.Sphere); //circle? how to slap this on the plane? or maybe let them overlap each other on the Z axis
				
				// static
				obj.transform.position = new Vector3(x, y, plane.center.z);

                objectList.Add(obj);

			}

        }
    }
		
	public void StepQuadtreeTestAdd()
	{
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere); // create one new object
		item.transform.position = GenerateRandomPosition(gamePlaneBounds);
			
		objectList.Add(item);
		quadtree.Insert(item);
		Debug.Log(item.transform.position);
		Debug.Log(quadtree.DisplayItems());
	}
		
	public void StepQuadtreeGridTestAdd()
	{
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere); // create one new object
		item.transform.position = GenerateGridPosition(gamePlaneBounds);
			
		objectList.Add(item);
		quadtree.Insert(item);
		Debug.Log(item.transform.position);
		Debug.Log(quadtree.DisplayItems());
	}
		
	public void StepSpatialHashTestAdd()
	{
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere); // create one new object
		item.transform.position = GenerateRandomPosition(gamePlaneBounds);
			
		objectList.Add(item);
		//spatialHash.Insert(item);
		Debug.Log(item.transform.position);
		Debug.Log(spatialHash.DisplayItems());
	}
		
	public void StepSpatialHashGridTestAdd()
	{
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere); // create one new object
		item.transform.position = GenerateGridPosition(gamePlaneBounds);
			
		objectList.Add(item);
		//spatialHash.Insert(item);
		Debug.Log(item.transform.position);
		Debug.Log(spatialHash.DisplayItems());
	}
		
	public void StepTestRemove()
	{
		GameObject item = objectList[objectList.Count];
		quadtree.Remove(item);
		objectList.Remove(item);
		Debug.Log(item.transform.position);
		Debug.Log(quadtree.DisplayItems());
	}
		
		
		
	public void Test1() // randomly positioned objects
	{
        InitialiseTestObjects(20);

		foreach (GameObject item in objectList)
        {
            quadtree.Insert(item);
        }
		quadtree.DebugDisplayNodes();
	}
		
	public void Test2() // Test1, then move a few objects, then rebuild quadtree
	{
		// equal grid format, move a few object and check if it is correctly reflected
			
		int RandomChangeCount = 5;
			
		Test1();
			
		for(int i = 0; i < RandomChangeCount; i++)
		{
            objectList[(int)Mathf.Floor(UnityEngine.Random.Range(0, objectList.Count))].transform.position = GenerateRandomPosition(gamePlane.GetComponent<Collider>().bounds);
		}
			
		foreach (GameObject item in objectList)
        {
            quadtree.Insert(item);
        }
		quadtree.DebugDisplayNodes();
	}
		
	public void Test3() // all objects positioned in a grid
	{
		InitializeGridObjects();
			
		foreach (GameObject item in objectList)
        {
            quadtree.Insert(item);
        }
		quadtree.DebugDisplayNodes();
	}
		
	public void Test4()
	{
		
	}
}