using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MakeCrossRoads : MonoBehaviour
{
    public int crossRoadStartPoint;
    public int crossRoadFirstSectionPoint;
    public int crossRoadSecondSectionPoint;
    public int crossRoadFirstEndPoint;
    public int crossRoadFirstConnectMainWayIndex;
    private int[] crossRoadCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CrossRoadsAlgorithm(Enemy enemy)
    {
        int selectedWayIndex = 1;
        crossRoadCount = new int[2];
        crossRoadCount[0] = crossRoadFirstSectionPoint;
        crossRoadCount[1] = crossRoadSecondSectionPoint;
        if(Random.Range(0f, 1f) >= 0.5f && Random.Range(0f, 1f) <= 1f)
        {
            selectedWayIndex = 1;
        }else
        {
            selectedWayIndex = 0;
        }
        enemy.MakeCrossRoad(crossRoadStartPoint, crossRoadCount[selectedWayIndex],
            crossRoadFirstEndPoint,crossRoadFirstConnectMainWayIndex);
    }
}
