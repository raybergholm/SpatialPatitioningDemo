using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using SpatialPartitioning;

public class TestScripts : MonoBehaviour
{
    enum TestTypes
    {
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

    private GameObject GenerateItemRandom()
    {
        return new GameObject();
    }
}