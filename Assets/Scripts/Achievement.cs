using System.Collections;
using UnityEngine;

public interface IObserver
{
    public void OnNotify(string name, int progress);
}

[System.Serializable]
public class Achievement : IObserver
{
    private Hashtable achievements = new Hashtable();

    public void createAchievement(string name, int progress)
    {
        achievements.Add(name, progress);
    }

    private void updateAchievement(string name, int progress)
    {
        int currentProgress = (int) achievements[name];

        achievements[name] = currentProgress + progress;
    }

    public int GetAchievement(string name)
    {
        return (int) achievements[name];
    }

    void IObserver.OnNotify(string name, int progress)
    {
        updateAchievement(name, progress);
    }
}
