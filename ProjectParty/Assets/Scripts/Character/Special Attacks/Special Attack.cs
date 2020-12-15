using System;
using System.Collections;

//     Not used anymore 
public interface SpecialAttack
{
    float Energy { get; }
    bool IsReady { get; }

    IEnumerator use();


}
