using UnityEngine;
using Mirror;

// Custom NetworkManager that simply assigns the correct racket positions when
// spawning players. The built in RoundRobin spawn method wouldn't work after
// someone reconnects (both players would be on the same side).
[AddComponentMenu("")]
public class NetworkManagerPong : NetworkManager
{
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;
    public enum Side { Left, Right }
    [SerializeField] private Side side;
    GameObject ball;

    private int leftScore;
    private int rightScore;
    private Hud hud;
    private GameObject hudCanvas;

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
            NetworkServer.Spawn(ball);
            hudCanvas = GameObject.Find("HUD_Canvas");
            hud = hudCanvas.GetComponent<Hud>();
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
    public void LeftPoint()
    {
        leftScore++;
        hud.leftScore.text = leftScore.ToString();
    }
    public void RightPoint()
    {
        rightScore++;
        hud.leftScore.text = rightScore.ToString();
    }
}