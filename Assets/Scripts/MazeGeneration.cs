using UnityEngine;
using UnityEngine.AI;

public class MazeGeneration : MonoBehaviour
{
    public GameObject[] planes;

    public GameObject wall;

    public GameObject player;

    private const float _wallSpawnChange = 0.2f;

    private bool _playerSpawned;
    
    private readonly Vector3 _wallOffset = new Vector3(4.5f,-0.5f,4.5f);
    private readonly Vector3 _playerOffset = new Vector3(4.5f,0.7f,4.5f);
    
    public void Start()
    {
        //Two foreaches are used because otherwise the NavMeshSurfaces will not bake correctly.
        foreach (var plane in planes) {
            GenerateMaze(plane);
        }

        foreach (var plane in planes) {
            BakeNavigationMesh(plane);          
        }
    }

    private void GenerateMaze(GameObject plane)
    {
        const int numCols = 10;
        const int numRows = 10;

        for (int col = 0; col < numCols; col++) {
            for (int row = 0; row < numRows; row++) {
                if (!_playerSpawned) {
                    InstantiatePlayer(plane, col, row);
                } else {
                    float diceRoll = Random.Range(0.0f, 1.0f);
                    if (!(diceRoll <= _wallSpawnChange)) continue;
                    InstantiateWall(plane, col, row);                    
                }
            }
        }
    }
    
    private static void BakeNavigationMesh(GameObject plane) 
    {
        NavMeshSurface navMeshSurface = plane.GetComponent(typeof(NavMeshSurface)) as NavMeshSurface;
        if (navMeshSurface != null) navMeshSurface.BuildNavMesh();
    }

    private void InstantiateWall(GameObject plane, int col, int row)
    {
        var wallOffset = new Vector3(col, 0, row);
        var wallPosition = wallOffset + plane.transform.position - _wallOffset;
        var wallGameObject = Instantiate(wall, wallPosition, Quaternion.identity);
        wallGameObject.transform.SetParent(plane.transform);
    }

    private void InstantiatePlayer(GameObject plane, int col, int row)
    {
        var wallOffset = new Vector3(col, 0, row);
        var playerPosition = plane.transform.position + _playerOffset + wallOffset;
        Instantiate(player, playerPosition, Quaternion.identity);
        _playerSpawned = true;
    }
}
