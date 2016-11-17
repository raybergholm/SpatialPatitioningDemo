using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;

namespace SpatialPartitioning
{
	public class SimMenuScreen : MonoBehaviour
	{
		// TODO: may need to be set up in the Unity environment first
		public Canvas menuCanvas;
		public Button startSim;
		public Button resetSim;
		
		
		void Start()
		{
			menuCanvas = menuCanvas.GetComponent<Canvas>();
			startSim = startSim.GetComponent<Button> ();
			resetSim = resetSim.GetComponent<Button> ();
			menuCanvas.enabled = false;
		}
		
		void Update()
		{
			
		}
		
		void FixedUpdate()
		{
			
		}
	}
}