using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelDatabaseManager : MonoBehaviour
{
    [SerializeField] private string levelName;

    public void CreateNewLevel(List<PlacementData> objectDatas)
    {
        List<Object> objectLists = new();
        
        foreach (var data in objectDatas)
        {
            Vector _tempPos = new(data.occupiedPositions.x, data.occupiedPositions.y, data.occupiedPositions.z);
            Vector _tempRot = new(data.occupiedPositions.x, data.occupiedPositions.y, data.occupiedPositions.z);
            Object _tempObj = new(data.ID, _tempPos, _tempRot);

            objectLists.Add(_tempObj);
        }

        // TODO: Convert List to JSON
        string json = JsonUtility.ToJson(objectLists);

        // TODO: Save List to Firebase (download Firebase Database SDK duls)
    }

    public void EditLevel()
    {
        // TODO : Get Level from database


    }

    
}

public class Object
{
    public int iD;
    public Vector position;
    public Vector rotation;

    public Object(int iD, Vector position, Vector rotation)
    {
        this.iD = iD;
        this.position = position;
        this.rotation = rotation;
    }
}

public class Vector
{
    public float x;
    public float y;
    public float z;

    public Vector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}