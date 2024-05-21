using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    PickupManager pickup;

    private GameObject objectSpawned;
    private List<GameObject> allObjectsSpawned = new List<GameObject>();

    [SerializeField] List<GameObject> negativeMechanicPrefabs;

    [SerializeField] List<GameObject> spawnPointsRow1;
    [SerializeField] List<GameObject> spawnPointsRow2;
    [SerializeField] List<GameObject> spawnPointsRow3;
    [SerializeField] List<GameObject> spawnPointsRow4;
    [SerializeField] List<GameObject> spawnPointsRow5;
    private List<List<GameObject>> spawnRows = new List<List<GameObject>>();

    private void Start()
    {
        pickup = GetComponent<PickupManager>();

        spawnRows.Add(spawnPointsRow1);
        spawnRows.Add(spawnPointsRow3);
        spawnRows.Add(spawnPointsRow2);
        spawnRows.Add(spawnPointsRow4);
        spawnRows.Add(spawnPointsRow5);

        //negativeMechanics();

        int spawnAmount = FindObjectOfType<GameManager>().obstaclesToSpawn; // 1-21 (need spots empty for player)

        foreach (List<GameObject> spawnRow in spawnRows)
        {
            List<GameObject> spawnPoints = spawnRow;
            int _spawnAmount = spawnAmount;
            GameObject mechanicToSpawn = negativeMechanicPrefabs[Random.Range(0, negativeMechanicPrefabs.Count)]; //choose random negative mechanic to spawn

            if (/*spawnAmount >= 6 && */Random.value > 0.6f) //spawn random negative mechanic if true
            {
                int amountOfRemovedIndices = 0;
                List<int> occupiedIndices = new List<int>();
                bool horizontal;

                switch (mechanicToSpawn.tag)
                {
                    case "Mine":
                        int indexToSpawnMine = Random.Range(0, spawnPoints.Count); //choose random index to spawn the mine at
                        objectSpawned = Instantiate(mechanicToSpawn, spawnPoints[indexToSpawnMine].transform.position, Quaternion.identity); //spawn mine and store it
                        allObjectsSpawned.Add(objectSpawned); //add mine to list of all objects spawned for simple deletion later on
                        FindAllIndicesForMine(indexToSpawnMine, spawnPoints, out occupiedIndices, out amountOfRemovedIndices);
                        break;
                    case "LaserEnemy":
                        int indexToSpawnLaser;
                        float zRotation;
                        GetRandomLaserEnemyIndex(out indexToSpawnLaser, out zRotation, out horizontal);
                        objectSpawned = Instantiate(mechanicToSpawn, spawnPoints[indexToSpawnLaser].transform.position, Quaternion.identity);
                        objectSpawned.GetComponent<LaserEnemy>().SetDirection(zRotation);
                        allObjectsSpawned.Add(objectSpawned);
                        FindAllIndicesForLaser(indexToSpawnLaser, horizontal, out occupiedIndices, out amountOfRemovedIndices);
                        break;
                    case "MagneticRock":
                        int indexToSpawnFirstRock;
                        int indexToSpawnSecondRock;
                        GameObject secondRock;
                        GetRandomMagneticRockIndex(out indexToSpawnFirstRock, out indexToSpawnSecondRock, out horizontal);
                        objectSpawned = Instantiate(mechanicToSpawn, spawnPoints[indexToSpawnFirstRock].transform.position, Quaternion.identity);
                        objectSpawned.GetComponent<MagneticRock>().SpawnSecondRock(horizontal, spawnPoints[indexToSpawnSecondRock].transform.position, out secondRock);
                        allObjectsSpawned.Add(objectSpawned);
                        allObjectsSpawned.Add(secondRock);
                        FindAllIndicesForMagneticRock(indexToSpawnFirstRock, horizontal, out occupiedIndices, out amountOfRemovedIndices);
                        break;
                }

                List<GameObject> newSpawnPoints = new List<GameObject>(); //declare new temp list

                for (int i = 0; i < spawnPoints.Count; i++) //store all indices that arent occupied into the new temp list
                {
                    bool addIndex = true;
                    foreach (int index in occupiedIndices)
                    {
                        if (i == index)
                        {
                            addIndex = false;
                            break;
                        }
                    }
                    if (addIndex)
                        newSpawnPoints.Add(spawnPoints[i].gameObject);
                }

                spawnPoints = newSpawnPoints; //set the new list of spawn points with the correct ones removed
                _spawnAmount -= amountOfRemovedIndices; //subtract from amount of spawned obstacles by mine and surrounding index count
                Mathf.Clamp(_spawnAmount, 0, spawnAmount);
            }

            if (spawnAmount >= 8)
                _spawnAmount = Random.Range(_spawnAmount - 2, _spawnAmount + 2);

            Mathf.Clamp(_spawnAmount, 0, spawnAmount);

            for (int i = 0; i < _spawnAmount; i++)
            {
                int indexToSpawnObstacle = Random.Range(0, spawnPoints.Count);
                objectSpawned = Instantiate(obstaclePrefab, spawnPoints[indexToSpawnObstacle].transform.position, Quaternion.identity);
                allObjectsSpawned.Add(objectSpawned);
                spawnPoints.RemoveAt(indexToSpawnObstacle);
            }

            if (Random.value > 0.8f) //spawn random power up if true
            {
                int indexToSpawnPickup = Random.Range(0, spawnPoints.Count);
                pickup.SpawnPickup(spawnPoints[indexToSpawnPickup].transform.position);
            }
        }
    }
    private void FindAllIndicesForMine(int selectedIndex, List<GameObject> spawnPoints, out List<int> occupiedIndices, out int removedIndices)
    {
        //First get the amount of surrounding squares (indices) around the selected index. This also includes the index itself.
        int selectedRow = selectedIndex / 5;
        int selectedColumn = selectedIndex % 5;
        List<int> _occupiedIndices = new List<int>();

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                int newRow = selectedRow + rowOffset;
                int newColumn = selectedColumn + colOffset;

                // Check if the new position is within the grid boundaries (5x5)
                if (newRow >= 0 && newRow < 5 && newColumn >= 0 && newColumn < 5)
                {
                    int neighborElement = newRow * 5 + newColumn;
                    _occupiedIndices.Add(neighborElement);
                }
            }
        }


        occupiedIndices = _occupiedIndices;
        removedIndices = _occupiedIndices.Count;
    }

    private void GetRandomLaserEnemyIndex(out int index, out float zRotation, out bool horizontal)
    {
        if (Random.value > 0.5f)
            horizontal = true;
        else
            horizontal = false;

        List<int> edgeAndCornerSquares = new List<int>();

        // Add corner squares because they can be horizontal or vertical
        edgeAndCornerSquares.Add(0);
        edgeAndCornerSquares.Add(4);
        edgeAndCornerSquares.Add(20);
        edgeAndCornerSquares.Add(24);

        if (horizontal)
        {
            // Add edge squares (left and right)
            for (int i = 1; i < 4; i++)
            {
                edgeAndCornerSquares.Add(i * 5);
                edgeAndCornerSquares.Add(i * 5 + 4);
            }
        }
        else //vertical
        {
            // Add edge squares (top and bottom)
            for (int i = 1; i < 4; i++)
            {
                edgeAndCornerSquares.Add(i);
                edgeAndCornerSquares.Add(20 + i);
            }
        }

        //get random index from usable indices
        index = edgeAndCornerSquares[Random.Range(0, edgeAndCornerSquares.Count)];

        if (horizontal)
        {
            if (index % 5 == 0)
                zRotation = 0f;
            else
                zRotation = 180f;
        }
        else
        {
            if (index < 5)
                zRotation = 90f;
            else
                zRotation = -90f;
        }
    }

    private void FindAllIndicesForLaser(int selectedIndex, bool horizontal, out List<int> occupiedIndices, out int removedIndices)
    {
        List<int> _occupiedIndices = new List<int>();

        if (horizontal)
        {
            if (selectedIndex % 5 == 4)
                selectedIndex -= 4;

            for (int i = 0; i < 5; i++)
            {
                _occupiedIndices.Add(selectedIndex + i);
            }
        }
        else
        {
            if (selectedIndex > 19)
                selectedIndex -= 20;

            for (int i = 0; i < 5; i++)
            {
                _occupiedIndices.Add(selectedIndex + (i * 5));
            }
        }

        occupiedIndices = _occupiedIndices;
        removedIndices = _occupiedIndices.Count;
    }

    private void GetRandomMagneticRockIndex(out int index, out int secondRockIndex, out bool horizontal)
    {
        if (Random.value > 0.5f)
            horizontal = true;
        else
            horizontal = false;

        List<int> spawnableSquares = new List<int>();

        if (horizontal)
        {
            for (int i = 0; i < 5; i++)
            {
                spawnableSquares.Add(i * 5);
                spawnableSquares.Add(1 + (i * 5));
            }
        }
        else //vertical
        {
            for (int i = 0; i < 10; i++)
            {
                spawnableSquares.Add(i);
            }
        }

        //get random index from usable indices
        index = spawnableSquares[Random.Range(0, spawnableSquares.Count)];

        if (horizontal)
            secondRockIndex = index + 3;
        else
            secondRockIndex = index + 15;
    }

    private void FindAllIndicesForMagneticRock(int selectedIndex, bool horizontal, out List<int> occupiedIndices, out int removedIndices)
    {
        List<int> _occupiedIndices = new List<int>();

        if (horizontal)
        {
            for (int i = 0; i < 4; i++)
            {
                _occupiedIndices.Add(selectedIndex + i);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                _occupiedIndices.Add(selectedIndex + (i * 5));
            }
        }

        occupiedIndices = _occupiedIndices;
        removedIndices = _occupiedIndices.Count;
    }

    public void RemoveSpawnedObstacles()
    {
        foreach (var gameObject in allObjectsSpawned)
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }
}
