using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MovingObject {

    public int wallDamage = 1; // How much damage to walls
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1;
    public Text foodText;

    private Animator animator;
    private int food; // Score score during level

	// Use this for initialization
	protected override void Start () { // override movingobject start
        animator = GetComponent<Animator> ();
        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;
        base.Start();
	}

    private void OnDisable () {
        GameManager.instance.playerFoodPoints = food;
    }
        
    void Update () {
        if (!GameManager.instance.playersTurn) {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw ("Horizontal");
        vertical = (int)Input.GetAxisRaw ("Vertical");

        if (horizontal != 0) {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) {
            AttemptMove<Wall> (horizontal, vertical); // We expect to interact with a wall as a player
        }
    }

    protected override void AttemptMove <T> (int xDir, int yDir) {
        --food;
        foodText.text = "Food: " + food;
        base.AttemptMove <T> (xDir, yDir);

        RaycastHit2D hit; // reference result of linecast done in Move
        CheckIfGameOver ();

        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Exit") {
            Invoke ("Restart", restartLevelDelay);
            enabled = false;
        } else if (other.tag == "Food") {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            other.gameObject.SetActive (false);
        } else if (other.tag == "Soda") {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            other.gameObject.SetActive (false);
        }
    }

    protected override void OnCantMove <T> (T component) {
        Wall hitWall = component as Wall; // cast to Wall
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger ("playerChop");
    }

    private void Restart () {
        // reload level if player goes to exit
        SceneManager.LoadScene(0);
    }

    public void LoseFood (int loss) {
        animator.SetTrigger ("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver ();
    }

    private void CheckIfGameOver () {
        if (food <= 0) {
            GameManager.instance.GameOver ();
        }
    }
}
