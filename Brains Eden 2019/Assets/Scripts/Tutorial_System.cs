using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_System : MonoBehaviour
{
    [SerializeField] ChainManager chain_man;

    enum States {BLANK,STICK_ENEMIES,STICK_BOMBS,DAMAGE_SPELLS,END_MESSAGE,START }
    States current_state = States.BLANK;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_state)
        {
            case States.BLANK:
                {

                    break;
                }
            case States.STICK_ENEMIES:
                {

                    break;
                }
            case States.STICK_BOMBS:
                {

                    break;
                }
            case States.DAMAGE_SPELLS:
                {

                    break;
                }
            case States.END_MESSAGE:
                {

                    break;
                }
            case States.START:
                {

                    break;
                }
        }
    }
}
