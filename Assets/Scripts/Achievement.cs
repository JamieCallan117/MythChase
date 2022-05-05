using System.Collections;
using UnityEngine;

//Observer pattern
public interface IObserver
{
    public void OnNotify(string name, int progress);
}

//Achievement class for storing the hashtable of achievements and adding/updating them.
//Serializable as it gets saved to a file.
[System.Serializable]
public class Achievement : IObserver
{
    private Hashtable achievements = new Hashtable();

    //Used when achievements file first created.
    public void createAchievement(string name, int progress)
    {
        achievements.Add(name, progress);
    }

    //Updates the values of the specified achievement.
    private void updateAchievement(string name, int progress)
    {
        int currentProgress = (int) achievements[name];

        achievements[name] = currentProgress + progress;
    }

    //Returns the value of an achievement.
    public int GetAchievement(string name)
    {
        return (int) achievements[name];
    }

    //Observer gets notified to update an achievement.
    void IObserver.OnNotify(string name, int progress)
    {
        updateAchievement(name, progress);
    }
}
