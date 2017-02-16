using UnityEngine;
using System.Collections.Generic;
 
public class Keeper : Character
{

    private short hunger = 100;
    private short mentalHealth = 100;
 
    public short Hunger {
        get { return hunger; }
        set { hunger = value; }
    }

    public short MentalHealth {
        get { return mentalHealth; }
        set { mentalHealth = value; }
    }

    public Keeper()
    {
    }
    
}
