using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f; // how long to wait between turns
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText; // Day X
    private GameObject levelImage;

    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup; // don;t let player move during board setup

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject); // Make the manager persist between scenes
        enemies = new List<Enemy> ();
        boardScript = GetComponent<BoardManager> ();
        InitGame ();
    }

    private void OnLevelWasLoaded (int index) {
        ++level;
        InitGame ();
    }

    void InitGame () {
        doingSetup = true;
        levelImage = GameObject.Find ("LevelImage");
        levelText = GameObject.Find ("LevelText").GetComponent <Text> ();
        levelText.text = "Day " + level;
        levelImage.SetActive (true);
        Invoke ("HideLevelImage", levelStartDelay);

        enemies.Clear (); // gamemanager isn't reset so clear out enemies from last level
        boardScript.SetupScene (level);
    }

    private void HideLevelImage () {
        levelImage.SetActive (false);
        doingSetup = false;
    }

    public void GameOver () {
        levelText.text = "After " + level + " days,\n you starved!";
        levelImage.SetActive (true);
        enabled = false;
    }

    void Update () {
        if (playersTurn || enemiesMoving || doingSetup) {
            return;
        }

        StartCoroutine (MoveEnemies ());
    }

    public void AddEnemyToList (Enemy script) {
        enemies.Add (script);
    }

    IEnumerator MoveEnemies () {
        enemiesMoving = true;
        yield return new WaitForSeconds (turnDelay);
        if (enemies.Count == 0) {
            yield return new WaitForSeconds (turnDelay);
        }

        for (int i = 0; i < enemies.Count; ++i) {
            enemies [i].MoveEnemy ();
            yield return new WaitForSeconds (enemies [i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

}
