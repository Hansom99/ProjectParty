using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public interface Weapon
{
    float ShotsPerSecound { get; set; }

    int Ammunition { get; set; }

    void showShot(Vector3 endpoint);
    void attack();
    void reload();

}
