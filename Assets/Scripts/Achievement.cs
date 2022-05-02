using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement {
    public Hashtable achievements = new Hashtable();

    public void createAchievement(string name, int progress) {
        achievements.Add(name, progress);
    }

    public void updateAchievement(string name, int progress) {
        achievements[name] = progress;
    }
}
