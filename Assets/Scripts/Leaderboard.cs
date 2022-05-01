using System.Collections;

[System.Serializable]
public class Leaderboard {
    public Hashtable leaderboard = new Hashtable();

    public void addScore(string name, int score) {
        leaderboard.Add(name, score);
    }

    public void updateScore(string name, int score) {
        leaderboard[name] = score;
    }
}
