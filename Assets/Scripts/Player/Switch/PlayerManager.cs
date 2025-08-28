using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player[] players;
    private int currentIndex = 0;

    private void Start()
    {
        players[0].SetControlled(true);   
        players[1].SetControlled(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            players[currentIndex].SetControlled(false);

            currentIndex = (currentIndex + 1) % players.Length;

            players[currentIndex].SetControlled(true);
        }
    }
}

