using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public interface Weapon
{

    bool isSpikes { get; set; }

    bool isGun { get; set; }

    float shotsPerSecound { get; set; }

    int ammunition { get; set; }

    int maxAmmu { get; set; }

    void showShot(Vector3 endpoint);
    void attack();
    void reload();

}
