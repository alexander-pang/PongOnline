using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        SpriteRenderer sprite;
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        public int serverScore;
        public int clientScore;
        public Color onServer = new Color(1, 0, 0);
        public Color offServer = new Color(0, 0, 1);
        public Color onClient = new Color(0, 1, 0);
        public Color offClient = new Color(1, .5f, 1);
        public bool serverToggle = false;
        public bool clientToggle = false;

        public bool hasStarted = true;

        //public Text leftScoreText;
        //public Text rightScoreText;

        //public Text initialValue = Text
        //public Hud hud = new Hud("0","0","0","0");

        // need to use FixedUpdate for rigidbody
        void FixedUpdate()
        {
            
                // only let the local player control the racket.
                // don't control other player's rackets
            if (isLocalPlayer)
            {
                //if the paddle is on the machine that is playing
                rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
                if (hasStarted)
                {
                    //changes the server paddle color to blue on the server screen
                    //doesn't affect the client paddle color on the server screen
                    //changes the client paddle color to blue on the client screen
                    //doesn't affect the server paddle color on the client screen
                    //racketColor = new Color(0, 0, 1);
                    sprite = GetComponent<SpriteRenderer>();
                    //sprite.color = racketColor;
                    serverScore = 0;
                    clientScore = 0;
                    hasStarted = false;
                }
            }
  
        }

        public void ChangeColor(SpriteRenderer aSprite, Color aColor)
        {
            aSprite.color = aColor;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (isLocalPlayer)
            {
                //changes server paddle on the server screen
                //racketColor = new Color(0, 1, 1);
                sprite = GetComponent<SpriteRenderer>();
                if (serverToggle)
                {
                    ChangeColor(sprite, onServer);
                    serverToggle = !serverToggle;
                    //serverScore++;
                    //Hud.instance.leftScore.text = serverScore.ToString();

                }
                else
                {
                    ChangeColor(sprite, offServer);
                    serverToggle = !serverToggle;
                    //serverScore++;
                    //Hud.instance.leftScore.text = serverScore.ToString();
                }
            }
            else
            {
                //changes the client paddle on the server screen
                //racketColor = new Color(1, 0, 0);
                sprite = GetComponent<SpriteRenderer>();
                if (clientToggle)
                {
                    ChangeColor(sprite, onClient);
                    clientToggle = !clientToggle;
                    //clientScore++;
                    //Hud.instance.rightScore.text = clientScore.ToString();

                }
                else
                {
                    ChangeColor(sprite, offClient);
                    clientToggle = !clientToggle;
                    //clientScore++;
                    //Hud.instance.rightScore.text = clientScore.ToString();
                }
            }
        }
    }

}