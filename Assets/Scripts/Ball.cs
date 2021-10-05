using UnityEngine;

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public float speed = 500f;

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetBall();
    }

    private void SetRandomTrajectory()
    {
        Vector2 force = Vector2.zero;
        //it will go either left or right
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        // this makes the vector not affected by any scaling
        this.rigidbody.AddForce(force.normalized * this.speed);
    }

    public void ResetBall ()
    {
        this.transform.position = Vector2.zero;
        this.rigidbody.velocity = Vector2.zero;
        
        // delay the stert of the game by 1s
        Invoke(nameof(SetRandomTrajectory), 1f);
    }
}
