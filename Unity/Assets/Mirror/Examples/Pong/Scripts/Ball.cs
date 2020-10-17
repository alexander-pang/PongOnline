using System;
using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Ball : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        SpriteRenderer sprite;

        [SyncVar(hook = nameof(SetBall))]
        Color ballColor;

        void SetBall(Color oldColor, Color newColor)
        {
            sprite = GetComponent<SpriteRenderer>();
            sprite.color = newColor; //new Color (0,0,1,1);
            //sprite.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        private void Update()
        {
            
        }

        public override void OnStartServer()
        {
            if (isLocalPlayer)
            {
                ballColor = new Color(1, 0, 0, 1);
            }
            else
            {
                ballColor = new Color(0, 1, 0, 1);
            }
            base.OnStartServer();

            // only simulate ball physics on server
            rigidbody2d.simulated = true;

            // Serve the ball from left player
            rigidbody2d.velocity = Vector2.right * speed;
            //ballColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }


        float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
        {
            // ascii art:
            // ||  1 <- at the top of the racket
            // ||
            // ||  0 <- at the middle of the racket
            // ||
            // || -1 <- at the bottom of the racket
            return (ballPos.y - racketPos.y) / racketHeight;
        }

        // only call this on server
        [ServerCallback]
        void OnCollisionEnter2D(Collision2D col)
        {
            // Note: 'col' holds the collision information. If the
            // Ball collided with a racket, then:
            //   col.gameObject is the racket
            //   col.transform.position is the racket's position
            //   col.collider is the racket's collider

            // did we hit a racket? then we need to calculate the hit factor
            if (col.transform.GetComponent<Player>())
            {
                // Calculate y direction via hit Factor
                float y = HitFactor(transform.position,
                                    col.transform.position,
                                    col.collider.bounds.size.y);

                sprite = GetComponent<SpriteRenderer>();
                Color newColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                SetBall(sprite.color, newColor);

                // Calculate x direction via opposite collision
                float x = col.relativeVelocity.x > 0 ? 1 : -1;

                // Calculate direction, make length=1 via .normalized
                Vector2 dir = new Vector2(x, y).normalized;

                // Set Velocity with dir * speed
                rigidbody2d.velocity = dir * speed;
            }
            else
            {
                sprite = GetComponent<SpriteRenderer>();
                float xpos = sprite.transform.position.x;
                float ypos = sprite.transform.position.y;
                if (Math.Abs(xpos + 24.5) < 1.5)
                {
                    //sprite.color = new Color(0, 1, 0, 1);
                    SetBall(sprite.color, sprite.color);
                }
                else if (Math.Abs(xpos - 24.5) < 1.5)
                {
                    //sprite.color = new Color(1, .92f, .016f, 1);
                    SetBall(sprite.color, sprite.color);
                }
            }
        }
    }
}
