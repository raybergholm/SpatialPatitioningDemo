using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using SpatialPartitioning;

public class TestScripts : MonoBehaviour
{
    enum TestTypes
    {
        SanityTest,
        RandomScatterTest,
        GridTest,
        StaticGridTest,
        ManualTest
    }

    private bool allowManualControls;

    private void Start()
    {
        allowManualControls = false;
    }

    private void Update()
    {
        if (allowManualControls)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                GameObject newItem = GenerateItemRandom();

            }
        }
    }

    public List<string> GetManifest()
    {
        return Enum.GetNames(typeof(TestTypes)).ToList();
    }

    public void SetupRandomScatterTest(int amount)
    {
        List<GameObject> newItems = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            newItems.Add(GenerateItemRandom());
        }
    }

    public void SetupGridTest(int amount)
    {
        for (int i = 0; i < amount; i++)
        {

        }
    }

    public void SetupStaticGridTest()
    {

    }

    public void SetupManualTest()
    {

    }

    public void DoSanityTest()
    {
        // test result if no items

        // add one item to top left corner, test it went there (can just check each node's bounds)

        // add one item to each other corner, test it work correctly

        // add items to top left until it hits limit, test its all there

        // add one more item to top left, it should split the results down into subnodes

        // spray a load of random items in, test that everything went into the right node (bounds check for each node)
    }

    private GameObject GenerateItemRandom()
    {
        return new GameObject();
    }
}