// default template of the main Unity file

using UnityEngine;
using System.Collections;
/*
	for coordinates, use the GUI standard: (0,0) is top left, x grows rightwards and y grows downwards.

	// Rect documentation: http://docs.unity3d.com/ScriptReference/Rect.html
	// some useful Rect members:
		center 	- Vector2	- (x,y) coordinate of the center point
		x		- float		- top left x coordinate position
		y		- float		- top left y coordinate position
		width	- float		- total width
		height	- float		- total height
		xMin	- float		- min x coordinate value (left edge)
		xMax	- float		- max x coordinate value (right edge)
		yMin	- float		- min y coordinate value (top edge)
		yMax	- float		- min y coordinate value (bottom edge)
*/

namespace SpatialPartitioning
{
	public class Circle
	{
		public Vector2 pos { get; set; }
		public float radius { get; set; }
	}
	
	public struct OBB
	{
		public Rect boundingBox;
		public Matrix3 rotation;
		
		public OOB(Rect boundingBox, Matrix3 rotation)
		{
			this.boundingBox = boundingBox;
			this.rotation = rotation;
		}
		
		public OOB(Rect boundingBox, float angle)
		{
			this.boundingBox = boundingBox;
			this.rotation = new Matrix3(
				cos(angle), -sin(angle), 0,
				sin(angle),  cos(angle), 0,
				0, 0 , 1);
		}
	}
	
	
	
	/*
		It may be a good idea to swap to Collider2D (subclasses: CircleCollider2D and BoxCollider2D (=AABB)) since they are meant for solid objects with collision detection built in.
	*/
	
    public enum Quadrants { Northeast, Northwest, Southwest, Southeast };

    public static class CollisionDetector
	{
		// static class for now, can change it later if necessary
		
		// TODO: the assumption here is that the Rect members are either public or at least gettable

		public static bool AabbToAabb(Rect first, Rect second)
		{
			// This returns true only if there is an overlap (i.e. collision)
			return first.xMin < second.xMax &&	// x-axis feasibility check: if this is false, the entire first object is located to the right of the second. It cannot overlap, so exit early.
				first.xMax > second.xMin &&		// x-axis overlap check: if this is true, there is an overlap in the x-axis position
				first.yMin < second.yMax &&		// y-axis feasibility check: same idea as in the x-axis feasibility check
				first.yMax > second.yMin;		// y-axis overlap check: if this is true, there is an overlap in the y-axis position
		}
		
		// alternative using Rect.Contains() (Most likely slower than the previous since it's using primitive float comparisions, while this one is creating Vector2D objects and calling a class method):
		public static bool AabbToAabbAlternative(Rect first, Rect second)
		{
			// bool Rect.Contains(Vector2 point) -> checks if a point is inside the Rect.
			return first.Contains(Vector2(second.xMin, yMin)) ||
				first.Contains(Vector2(second.xMin, yMax)) ||
				first.Contains(Vector2(second.xMax, yMin)) ||
				first.Contains(Vector2(second.xMax, yMax));
		}
		
		public static bool CircleToCircle(Circle first, Circle second)
		{
			// negatives get cancelled, squaredDistance becomes a scalar value
			float distanceX = first.x - second.x,
				distanceY = first.y - second.y,
				squaredDistance = distanceX * distanceX + distanceY * distanceY,
				squaredTotalRadius = first.radius * first.radius + second.radius * second.radius;
		
			// if the distance between points is less than the combined radii then there has to be an overlap.
			return squaredDistance < squaredTotalRadius; // use squared distances to avoid having to calculate square root
		}
		
	}

	public static class boundingVolumeConverter
	{
		public static Rect CircleToAabb(Circle circle) // the AABB is a square
		{
			// new (x,y) is the centre point minus the radius on both x and y axes. Width and height are double the radius
			Rect aabb = new Rect(circle.x - circle.radius, circle.y - circle.radius, circle.radius * 2, circle.radius * 2); // TODO: assumes that the signature is Rect(x, y, height, width)
		}
		
		public static Circle AabbToCircle(Rect rect)
		{
			// new (x,y) is the centre point 
			return new Circle(rect.center.x, rect.center.y, );
		}
	}


	public class Entity
	{
		protected Vector2 position;
		
		private Rect bounds { get; }
		
		
		public Entity()
		{
			position = new Vector2();
		}
		
		public Entity(Vector2 position)
		{
			this.position = position;
		}
		
		/*
		public void Transpose(Matrix matrix)
		{
			
		}
		*/
		
		public override string ToString()
		{
			return String.Format("({0},{1})", this.position.x, this.position.y); // string parameter subsitition. Also contains culture-specific formatting such as 1.23 vs 1,23, or {0:C2}  -> $1.23 or 1,23€
		}
	}
	
	public class MobileEntity : Entity // public inheritance only, no private/protected
	{
		public MobileEntity(){
		
		}
		public MobileEntity(position, vector){ }
		
		
	}
}
