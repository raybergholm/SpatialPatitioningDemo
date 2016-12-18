﻿using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SpatialPartitioning;
using SpatialPartitioning.Quadtree;
using SpatialPartitioning.SpatialHash;

public class TestScripts : MonoBehaviour
{
    enum TestTypes
    {
        RandomScatterTest,
        GridTest
    }

    public List<string> GetManifest()
    {
        return Enum.GetNames(typeof(TestTypes)).ToList();
    }

    public void RandomScatterTest()
    {

    }

    public void GridTest()
    {

    }
}