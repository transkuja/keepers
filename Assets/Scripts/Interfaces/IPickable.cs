using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Handler();

public interface IPickable{

    void Pick(int i = 0);
}
