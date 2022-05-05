using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Setting up observers.
public class Subject
{
    private IObserver achievementObserver;

    //Set the achievement observer as that's the only observer in the game.
    public void SetObserver(IObserver observer)
    {
        achievementObserver = observer;
    }

    //Notify the observer.
    public void Notify(string name, int progress)
    {
        achievementObserver.OnNotify(name, progress);
    }
}
