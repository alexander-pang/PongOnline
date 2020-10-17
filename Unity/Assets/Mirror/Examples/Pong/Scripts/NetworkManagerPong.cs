using UnityEngine;

namespace Mirror.Examples.Pong
{
    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]
    public class NetworkManagerPong : NetworkManager
    {
        public Transform leftRacketSpawn;
        public Transform rightRacketSpawn;
        GameObject ball;
        GameObject LeftWall;
        GameObject RightWall;
        SpriteRenderer sprite;

        Rigidbody2D rb;

        public Rigidbody2D rigidbody2d;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);

            // spawn ball if two players
            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                LeftWall = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Left Trigger"));
                RightWall = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Right Trigger"));
                NetworkServer.Spawn(ball);
                NetworkServer.Spawn(LeftWall);
                NetworkServer.Spawn(RightWall);
                //sprite = ball.GetComponent<SpriteRenderer>();
                //sprite.color = new Color(1, .92f, .016f, 1);
                //sprite.size.Set(5, 5);
                //rb = ball.GetComponent<Rigidbody2D>();
                //rb.velocity = Vector2.right * 0;

            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // destroy ball
            if (ball != null)
                NetworkServer.Destroy(ball);

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
    }
}
