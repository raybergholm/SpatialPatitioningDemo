using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpatialPartitioning.BoundingVolumes;

namespace SpatialPartitioning
{
    public class CollidableEntity : MonoBehaviour
    {
        protected Bounds aabb;
        public Bounds AABB { get { return aabb; } }

        protected Vector3 movementVector;
        public Vector3 MovementVector { get { return movementVector; } }

        protected virtual void Awake()
        {
            aabb = new Bounds(gameObject.transform.position, new Vector3()); // TODO: best way to assign the proper size based on the attached game object?
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }
    }
}
