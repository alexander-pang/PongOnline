using System;
using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Ball : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        SpriteRenderer sprite;

        
        public Color onServer = new Color(1, 0, 0);
        public Color offServer = new Color(0, 0, 1);
        public Color onClient = new Color(0, 1, 0);
        public Color offClient = new Color(1, .5f, 1);
        public bool serverToggle = false;
        public bool clientToggle = false;

        public bool hasStarted = true;
        public Hud hud;

        [SyncVar(hook = nameof(CmdSetBall))]
        Color ballColor;

        [SyncVar(hook = nameof(CmdLeftScore))]
        public int serverScore=0;

        [SyncVar(hook = nameof(CmdRightScore))]
        public int clientScore=0;

        [Command]
        void CmdLeftScore(int oldScore, int newScore)
        {
            serverScore = newScore;
            Update();
        }

        [Command]
        void CmdRightScore(int oldScore, int newScore)
        {
            clientScore = newScore;
            Update();
        }

        private void Update()
        {
            Hud.instance.leftScore.text = serverScore.ToString();
            Hud.instance.rightScore.text = clientScore.ToString();
        }

        void CmdSetBall(Color oldColor, Color newColor)
        {
            sprite = GetComponent<SpriteRenderer>();
            sprite.color = newColor; //new Color (0,0,1,1);
            //sprite.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
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
                CmdSetBall(sprite.color, newColor);

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
                //if (Math.Abs(xpos + 24.5) < 1.5)
                
                if(col.transform.gameObject.tag == "Left")
                {
                    //sprite.color = new Color(0, 1, 0, 1);
                    CmdSetBall(sprite.color, sprite.color);

                    //client score incrementing by two, calling in server and client??
                    //handles all right score syncing
                    int prevScore = clientScore;
                    int newScore = clientScore +1;
                    clientScore= newScore;
                    //Hud.instance.rightScore.text = serverScore.ToString();
                    CmdRightScore(prevScore, newScore);
                }
               // else if (Math.Abs(xpos - 24.5) < 1.5)
                else if(col.transform.gameObject.tag == "Right")
                {
                    //sprite.color = new Color(1, .92f, .016f, 1);
                    CmdSetBall(sprite.color, sprite.color);

                    
                    //server score incrementing by two, calling in server and client??

                    //handles all left score syncing
                    int oldScore = serverScore;
                    int newServScore = serverScore +1;
                    serverScore = newServScore;
                    //Hud.instance.leftScore.text = serverScore.ToString();
                    //Hud.instance.leftScore.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
                    CmdLeftScore(oldScore,newServScore);
                    //Hud.instance.leftScore.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);

                }
            }
        }

    }
}
