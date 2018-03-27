using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Control : MonoBehaviour
{

    private Character_Movement mov;
    private Character_Fire attack;

    private Vector2 prevOrientation = Vector2.zero;

    void Start()
    {
        mov = GetComponent<Character_Movement>();
        if (mov == null)
        {
            throw new MissingComponentException("Expected a Character_Movement Component on " + gameObject);
        }

        attack = GetComponent<Character_Fire>();
        if (attack == null)
        {
            throw new MissingComponentException("Expected a Character_Fire Component on " + gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mov.move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        if (Input.GetAxisRaw("Roll") > 0)
        {
            mov.roll();
        }

        Vector2 orientation = new Vector2(Input.GetAxisRaw("LeftRight"), Input.GetAxisRaw("UpDown"));
        if (orientation.magnitude > 0.25f)
        {
            orientation.x = Mathf.Round(orientation.x);
            orientation.y = Mathf.Round(orientation.y);

            mov.rotate(orientation);
            //if (orientation != prevOrientation)
            //{
                attack.fire();
           // }
        }
        prevOrientation = orientation;
    }
}