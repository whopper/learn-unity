using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count (int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count (5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List <Vector3> gridPositions = new List<Vector3> ();

    // Setup the grid with a vector3 with all position positions (x, y ,0)
    void InitialiseList() {
        gridPositions.Clear ();
        // We're doing minus 1 to make sure the outer layer isn't impassable. Prevent
        // impossible levels.
        for (int x = 1; x < columns - 1; ++x) { // X-Axis
            for (int y = 1; y < rows - 1; ++y) { // Y-Axis
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Setup outer wall and floor of the game board
    void BoardSetup () {
        boardHolder = new GameObject ("Board").transform;

        // We're building an edge that the outer walls will go on,
        // hence the negative numbers that go to plus one.
        for (int x = -1; x < columns + 1; ++x) { // X-Axis
            for (int y = -1; y < rows + 1; ++y) { // Y-Axis
                GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
                // If we're in the range of outer wall tiles, choose an outer wall sprite
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject; // Cast to GameObject
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // Return a vector with a random grid location
    Vector3 RandomPosition () {
        int randomIndex = Random.Range (0, gridPositions.Count);
        Vector3 randomPosition = gridPositions [randomIndex];
        gridPositions.RemoveAt (randomIndex); // Remove that index from the matrix so we don't duplicate stuff on it.
        return randomPosition;
    }

    // Create all objects of a type at random locations on the board
    void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum) {
        int objectCount = Random.Range (minimum, maximum + 1); // Control how many of an object we'll have in a level
        for (int i = 0; i < objectCount; ++i) {
            Vector3 randomPosition = RandomPosition ();                             // Pick the location
            GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)]; // Pick the sprite
            Instantiate (tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level) {
        BoardSetup ();
        InitialiseList ();
        LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int)Mathf.Log (level, 2f); // Logarithmic increase by level
        LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
        Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity); // Exit is always in the upper right

    }
}
