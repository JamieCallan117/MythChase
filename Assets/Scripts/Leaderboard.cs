using System.Collections;

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
        if (leaderboard[name] == null)
        {
            return 0;
        }
        else
        {
            return (int) leaderboard[name];
        }
    }

    public Hashtable GetLeaderboards()
    {
        return leaderboard;
    }
}
