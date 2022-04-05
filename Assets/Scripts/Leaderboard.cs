using System.Collections;

[System.Serializable]
public class Leaderboard {
    private Hashtable leaderboard = new Hashtable();

    public void addScore(string name, int score) {
        leaderboard.Add(name, score);
    }
}
