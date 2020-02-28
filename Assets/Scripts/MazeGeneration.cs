using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vector2 = System.Numerics.Vector2;

public class MazeGeneration : MonoBehaviour
{
    public GameObject[] planes;

    public GameObject wall;

    public GameObject player;

    private bool _playerSpawned = false;

    private readonly Vector3 _wallOffset = new Vector3(4.5f,-0.5f,4.5f);
    private readonly Vector3 _playerOffset = new Vector3(4.5f,0.7f,4.5f);
    
    public void Start()
    {
        InstantiatePlayer(planes[0]);
        foreach (var plane in planes) {
            GenerateMaze(plane);
            NavMeshSurface navMeshSurface = plane.GetComponent(typeof(NavMeshSurface)) as NavMeshSurface;
            if (navMeshSurface != null) navMeshSurface.BuildNavMesh(); 
        }
        
    }

    private void GenerateMaze(GameObject plane)
    {
        const int numCols = 10;
        const int numRows = 10;
        
        var planeGrid = new int[numCols, numRows];

        for (int col = 0; col < numCols; col++) {
            for (int row = 0; row < numRows; row++) {
                var rand = Random.Range(0.0f, 1.0f);
                if (!(Random.Range(0.0f, 1.0f) <= 0.3f)) continue;
                if (plane.transform.name == "Plane1" && row == 10 && col == 10) continue;
                InstantiateWall(plane, col, row);
            }
        }
    }

    private void InstantiateWall(GameObject plane, int col, int row)
    {
        var wallOffset = new Vector3(col, 0, row);
        var wallPosition = wallOffset + plane.transform.position - this._wallOffset;
        var wallGameObject = Instantiate(wall, wallPosition, Quaternion.identity);
        wallGameObject.transform.SetParent(plane.transform);
    }

    private void InstantiatePlayer(GameObject plane)
    {
        var playerPosition = plane.transform.position + _playerOffset;
        Instantiate(player, playerPosition, Quaternion.identity);
    }
}
