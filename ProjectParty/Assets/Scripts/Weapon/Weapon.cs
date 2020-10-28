using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public interface Weapon
{
    float ShotsPerSecond { get; set; }

    int Ammunition { get; set; }

    /// <summary>
    /// Lässt den Schuss auf allen screens erscheinen.
    /// </summary>
    /// <param name="endpoint"></param>
    void showShot(Vector3 endpoint);

    /// <summary>
    /// berechnet, ob die Attacke getroffen hat und zieht Leben ab.
    /// </summary>
    void attack();

    /// <summary>
    /// Füllt Munition wieder auf.
    /// </summary>
    void reload();

}
