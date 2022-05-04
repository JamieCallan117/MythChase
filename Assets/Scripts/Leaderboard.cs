using System.Collections;
using UnityEngine;

[System.Serializable]
public class Leaderboard
{
    private Hashtable leaderboard = new Hashtable();

    public void addScore(string name, int score)
    {
        leaderboard.Add(name, score);
    }

    public void updateScore(string name, int score)
    {
        leaderboard[name] = score;
    }

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

    public Hashtable GetLeaderboards()
    {
        return leaderboard;
    }
}
