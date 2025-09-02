using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player[] players;
    private int currentIndex = 0;
    public int CurrentIndex => currentIndex;

    private void Start()
    {
        if (players.Length > 0)
            players[0].SetControlled(true);

        for (int i = 1; i < players.Length; i++)
            players[i].SetControlled(false);

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

