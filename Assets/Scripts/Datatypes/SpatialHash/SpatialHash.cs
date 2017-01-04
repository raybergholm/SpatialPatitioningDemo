using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{
    // works with point-like structures and AABB bounding volumes. Use Vector3 arguments for point-likes, or Bounds for AABB. Since bounding volumes occupy a volume of space, they can occupy multiple buckets, while point-likes can only occupy one at a time

    public class SpatialHash
    {
        private Dictionary<string, List<GameObject>> buckets;
        // granularity controls: number of cells = dimension length / bucketSize.
        private int bucketSizeX;
        private int bucketSizeY;
        private int bucketSizeZ;

        // Constructor
        public SpatialHash(int bucketSize) // buckets will be square/cubic
        {
            bucketSizeX = bucketSizeY = bucketSizeZ = bucketSize;

            buckets = new Dictionary<string, List<GameObject>>();
        }

        public SpatialHash(Vector3 bucketSize) // buckets will be whatever size specified
        {
            bucketSizeX = (int)bucketSize.x;
            bucketSizeY = (int)bucketSize.y;
            bucketSizeZ = (int)bucketSize.z;

            buckets = new Dictionary<string, List<GameObject>>();
        }

        public ICollection Keys { get { return buckets.Keys; } }

        public int BucketCount { get { return buckets.Count; } }

        public int ItemCount
        {
            get
            {
                int count = 0;
                foreach (List<GameObject> bucket in buckets.Values)
                {
                    count += bucket.Count;
                }
                return count;
            }
        }

        public bool Contains(Vector3 position)
        {
            return buckets.ContainsKey(ToKey(position));
        }

        // TODO: this is performing a shortcut and only testing the bucket in the centre. Should all buckets be tested?
        public bool Contains(GameObject item)
        {
            Bounds bounds = item.GetComponent<Collider>().bounds;
            string key = ToKey(bounds.center);

            return buckets.ContainsKey(key) ? buckets[key].Contains(item) : false;
        }

        public string ToKey(Vector3 position)
        {
            int bucketX = (int)Mathf.Floor(position.x / bucketSizeX) * bucketSizeX;
            int bucketY = (int)Mathf.Floor(position.y / bucketSizeY) * bucketSizeY;
            int bucketZ = (int)Mathf.Floor(position.z / bucketSizeZ) * bucketSizeZ;

            return bucketX.ToString() + "," + bucketY.ToString() + "," + bucketZ.ToString();
        }

        public List<string> ToKeys(Bounds bounds) // convert a given AABB to a list of all the buckets it encompasses
        {
            List<string> keys = new List<string>();

            int xMin = (int)Mathf.Floor((bounds.center.x - bounds.extents.x) / bucketSizeX) * bucketSizeX;
            int yMin = (int)Mathf.Floor((bounds.center.y - bounds.extents.y) / bucketSizeY) * bucketSizeY;
            int zMin = (int)Mathf.Floor((bounds.center.z - bounds.extents.z) / bucketSizeZ) * bucketSizeZ;

            int xMax = (int)Mathf.Floor((bounds.center.x - bounds.extents.x) / bucketSizeX) * bucketSizeX;
            int yMax = (int)Mathf.Floor((bounds.center.y + bounds.extents.y) / bucketSizeY) * bucketSizeY;
            int zMax = (int)Mathf.Floor((bounds.center.z + bounds.extents.z) / bucketSizeZ) * bucketSizeZ;

            for (int x = xMin; x < xMax; x += bucketSizeX)
            {
                for (int y = yMin; y < yMax; y += bucketSizeY)
                {
                    for (int z = zMin; z < zMax; z += bucketSizeZ)
                    {
                        keys.Add(x.ToString() + "," + y.ToString() + "," + z.ToString());
                    }
                }
            }

            return keys;
        }

        public List<GameObject> GetItems(Vector3 position)
        {
            string key = ToKey(position);
            return buckets.ContainsKey(key) ? buckets[key] : new List<GameObject>();
        }

        public List<GameObject> GetItems(Bounds bounds)
        {
            List<GameObject> items = new List<GameObject>();
            List<string> keys = ToKeys(bounds);

            foreach (string key in keys)
            {
                if (buckets.ContainsKey(key))
                {
                    items.AddRange(buckets[key]);
                }
            }

            return items.Distinct().ToList();
        }

        public void Insert(List<string> keys, GameObject item)
        {
            foreach (string key in keys)
            {
                if (!buckets.ContainsKey(key))
                {
                    buckets.Add(key, new List<GameObject>());
                }

                buckets[key].Add(item);
            }
        }

        public void Insert(Vector3 position, GameObject item)
        {
            string key = ToKey(position);

            if (!buckets.ContainsKey(key))
            {
                buckets.Add(key, new List<GameObject>());
            }

            buckets[key].Add(item);
        }

        public void Insert(Bounds bounds, GameObject item)
        {
            Insert(ToKeys(bounds), item);
        }

        public int Remove(List<string> keys, GameObject item)
        {
            int removedItemsCount = 0;

            foreach (string key in keys)
            {
                if (buckets.ContainsKey(key) && buckets[key].Remove(item))
                {
                    removedItemsCount++;
                }
            }
            return removedItemsCount;
        }

        public bool Remove(Vector3 position, GameObject item)
        {
            string key = ToKey(position);
            return buckets.ContainsKey(key) ? buckets[key].Remove(item) : false;
        }

        public int Remove(Bounds bounds, GameObject item)
        {
            return Remove(ToKeys(bounds), item);
        }

        public void Move(Bounds source, Bounds destination, GameObject item)
        {
            // when a BV moves, the buckets it occupies need to be updated too

            List<string> sourceKeys = ToKeys(source);
            List<string> destinationKeys = ToKeys(destination);


            List<string> removeFrom = sourceKeys.Except(destinationKeys).ToList(); // everything that exists in source but not destination = old positions no longer occupied, remove these
            List<string> insertTo = destinationKeys.Except(sourceKeys).ToList(); // everything that exists in destination but not source = new positions entirely, add these

            // Why not just remove the object entirely, then insert into the target buckets? Because if the object is barely moving (or not moving at all!) then most of the insert/remove calls are superfluous, and for multiple large objects that can add up to a lot of calls that use up computational time that could be used elsewhere.

            // Worse case scenario: removeFrom == sourceKeys && insertTo == destinationKeys means that the max number of Insert and Remove calls occurs here
            Remove(removeFrom, item);
            Insert(insertTo, item);
        }
    }
}