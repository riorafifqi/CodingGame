using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
    public void AddObjectAt(Vector3Int gridPosition, int iD, int placedObjectIndex)
    {
        PlacementData data = new PlacementData(gridPosition, iD, placedObjectIndex);
        if (placedObjects.ContainsKey(gridPosition))
            throw new System.Exception($"Dictionary already contains this cell position {gridPosition}");
        placedObjects[gridPosition] = data;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition))
        {
            return false;
        }

        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        placedObjects.Remove(gridPosition);
    }
}

public class PlacementData
{
    public Vector3Int occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(Vector3Int occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}