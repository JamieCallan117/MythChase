using System.Collections;
using UnityEngine;

//The leaderboard hashtable. Adds and updates scores per level.
[System.Serializable]
public class Leaderboard
{
    private Hashtable leaderboard = new Hashtable();

    //Adds a new score, unique username.
    public void addScore(string name, int score)
    {
        leaderboard.Add(name, score);
    }

    //Updates the score for an existing username.
    public void updateScore(string name, int score)
    {
        leaderboard[name] = score;
    }

    //Gets the score of an existing username, or -1 if they don't exist.
    public int GetScore(string name)
    {
        if (leaderboard.ContainsKey(name))
        {
            return (int) leaderboard[name];
        }
        else
        {
            return -1;
        }
    }

    //Get the leaderboard object.
    public Hashtable GetLeaderboards()
    {
        return leaderboard;
    }
}
