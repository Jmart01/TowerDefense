using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FloorTypes
{
    Grass,
    Water,
    Road
}
[ExecuteAlways]
public class Floor : MonoBehaviour
{
    [SerializeField] FloorTypes floorType = FloorTypes.Grass;
    [SerializeField] Material GrassMtl;
    [SerializeField] Material WaterMtl;
    [SerializeField] Material RoadMtl;
    public FloorTypes previousType;


    public void SetFloorType(FloorTypes newType)
    {
        if (newType != FloorTypes.Road)
            previousType = floorType;
        floorType = newType;
    }
    public FloorTypes GetFloorType()
    {
        return floorType;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            if(floorType != FloorTypes.Road)
            {
                previousType = floorType;
            }
            Material materialToUse = null;
            switch (floorType)
            {
                case FloorTypes.Grass:
                    materialToUse = GrassMtl;
                    break;
                case FloorTypes.Water:
                    materialToUse = WaterMtl;
                    break;
                case FloorTypes.Road:
                    materialToUse = RoadMtl;
                    break;
                default:
                    break;
            }
            GetComponentInChildren<MeshRenderer>().material = materialToUse;
        }
#endif
    }

    public void SetPreviousType()
    {
        floorType = previousType;
    }
}
