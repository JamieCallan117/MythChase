using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject
{
    IObserver achievementObserver;

    public void SetObserver(IObserver observer)
    {
        achievementObserver = observer;
    }

    public void Notify(string name, int progress)
    {
        achievementObserver.OnNotify(name, progress);
    }
}
