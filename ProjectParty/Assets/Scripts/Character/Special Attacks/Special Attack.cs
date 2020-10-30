using System;
using System.Collections;

public interface SpecialAttack
{
    float Energy { get; }
    bool IsReady { get; }

    IEnumerator use();


}
