using System;
using UnityEngine;
using Mirror;

namespace Mirror.Examples.Pong
{
    public class wall : NetworkBehaviour
    {
        SpriteRenderer sprite;
        public GameObject leftWall;
        public GameObject rightWall;

        // Update is called once per frame
        void Update()
        {

        }
        [ServerCallback]
        private void OnCollisionEnter2D(Collision2D col)
        {
            sprite = GetComponent<SpriteRenderer>();
            string wallHit = sprite.tag;
            if (wallHit == "Left")
            {
                sprite.color = new Color(0, 1, 0, 1);
            }
            else if (wallHit == "Right")
            {
                sprite.color = new Color(1, .92f, .016f, 1);

            }
        }
    }
}
