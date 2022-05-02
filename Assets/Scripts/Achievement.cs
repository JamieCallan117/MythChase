using System.Collections;

[System.Serializable]
public class Achievement
{
    private Hashtable achievements = new Hashtable();

    public void createAchievement(string name, int progress)
    {
        achievements.Add(name, progress);
    }

    public void updateAchievement(string name, int progress)
    {
        achievements[name] = progress;
    }

    public int GetAchievement(string name)
    {
        return (int) achievements[name];
    }
}
