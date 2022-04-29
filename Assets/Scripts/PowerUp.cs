using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    public int type;
    //1 | Ina. Teleport. Teleport to random area of map (hopefully). Maybe try teleporting to a node.
    //2 | Kiara. Invincibility. Unable to be eaten for a duration.
    //3 | Ame. Stop time. Freezes enemies for duration.
    //4 | Calli. Eat unfrightend enemies. Basically same as a large pellet, but they'll still come towards you.
    //5 | Gura. Scare enemies. Basically a useable large pellet.

    public void test() {
        print("Yahoo: " + type);
    }
}
