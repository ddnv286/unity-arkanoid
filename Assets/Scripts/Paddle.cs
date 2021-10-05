using UnityEngine;

public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public float speed = 50f;
    public float maxBounceAngle = 75f;

    private void Awake ()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ResetPaddle ()
    {
        this.transform.position = new Vector2(0, this.transform.position.y);
        this.rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.direction = Vector2.left;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.direction = Vector2.right;
        } else
        {
            this.direction = Vector2.zero;
        }
    }

    private void FixedUpdate ()
    {
        if (this.direction != Vector2.zero)
        {
            this.rigidbody.AddForce(this.direction * this.speed);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        
        if (ball != null)
        {
            Vector3 paddlePosition = this.transform.position;
            // get where the ball actually first touch the paddle
            Vector2 contactPoint = collision.GetContact(0).point;
            
            // 
            float offset = paddlePosition.x - contactPoint.x;
            // we only need half of the paddle's width 
            // and the rest of them can be accessed by negative or positive number
            // 'otherCollider' in this case is actually the paddle since 'collision' is assigned for the ball's collision
            float width = collision.otherCollider.bounds.size.x / 2;

            // we want signed angle so the angle can be positive or negative
            // velocity will give us the direction of the ball
            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rigidbody.velocity);
            // calculating the bounce angle as the contact point of the ball touch any percentage of one paddle's side
            // then bounce the ball upward by that percentage of the maxBounceAngle
            float bounceAngle = (offset / width) * this.maxBounceAngle;
            // guarantee that the angle which the ball goes upward never exceeds maxBounceAngle
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle);
            // we want the ball to rotate in whatever the forward direction would be
            Quaternion rotaion = Quaternion.AngleAxis(newAngle, Vector3.forward);
            // relative to the y axis and maintain the velocity
            ball.rigidbody.velocity = rotaion * Vector2.up * ball.rigidbody.velocity.magnitude;
        }
    }
}
