using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int LevelSize = new Vector2Int(10,10);
    [SerializeField] GameObject FloorPrefab;
    [SerializeField] LayerMask FloorLayerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public Vector3[] GetPathWaypoints()
    {
        LineRenderer Line = GetComponent<LineRenderer>();
        Vector3[] waypoints = new Vector3[Line.positionCount];
        Line.GetPositions(waypoints);
        return waypoints;
    }
    public void RebuildFloor()
    {
        Floor[] floors = GetComponentsInChildren<Floor>();
        foreach(var floor in floors)
        {
            DestroyImmediate(floor.gameObject);
        }

        for(int x=0; x < LevelSize.x; x++)
        {
            for (int z = 0; z < LevelSize.y; z++)
            {
                Instantiate(FloorPrefab, new Vector3(x,0,z), Quaternion.identity, transform);
            }
        }
    }

    public void SnapLinePointToFloor()
    {
        //finds the points of the line renderer
        LineRenderer Line = GetComponent<LineRenderer>();
        Vector3[] waypoints = new Vector3[Line.positionCount];
        Line.GetPositions(waypoints);
        
       
        for (int i = 0; i < waypoints.Length; i++)
        {
            Collider[] Cols = Physics.OverlapSphere(waypoints[i], 0.1f, FloorLayerMask);
            if (Cols.Length > 0)
            {
                Vector3 FloorPos = Cols[0].transform.position;
                //moves the point to the floor's center
                Line.SetPosition(i, new Vector3(FloorPos.x, 0f, FloorPos.z));
            }
        }
        //change the material of the blocks under the lines
        SetPreviousMtls();
        Line.GetPositions(waypoints);
        
        for(int i = 1; i < waypoints.Length; i++)
        {
            Collider[] Cols = Physics.OverlapCapsule( waypoints[i],waypoints[i-1],0.1f,FloorLayerMask);
            foreach(var col in Cols)
            {
                col.gameObject.GetComponentInParent<Floor>().SetFloorType(FloorTypes.Road);
            }
        }
    }

    public void SetPreviousMtls()
    {
        Floor[] floors = GetComponentsInChildren<Floor>();
        foreach (var floor in floors)
        {
            floor.SetPreviousType();
        }
    }
}
