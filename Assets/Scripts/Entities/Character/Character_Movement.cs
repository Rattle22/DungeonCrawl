using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : Entity_Movement {

    [SerializeField]
    private float rollMultiplier;
    [SerializeField]
    private int totalRollDuration;
    [SerializeField]
    private int totalRollCd;

    private Collider2D collider;

    private bool isRolling = false;
    private int rollCd = 0;
    private int rollDur = 0;
    private Vector2 rollDir = Vector2.zero;
    private float oldSpeed;


    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        speed = 5;
        collider = GetComponent<Collider2D>();
    }

    protected void Update() {
        updateRoll();
    }

    protected void updateRoll() {
        if (rollCd > 0 && !isRolling)
        {
            rollCd--;
        }

        if (isRolling)
        {
            rollDur--;
            speed = Mathf.Sin(((float)(totalRollDuration - rollDur) / totalRollDuration) * Mathf.PI) * rollMultiplier * oldSpeed;
            move(rollDir);
            if (rollDur < 0) {
                isRolling = false;
                speed = oldSpeed;
                gameObject.layer = 10;
            }
        }
    }

    //TODO: Invent ability system, port this there
    public void roll() {
        if (rollCd <= 0 && rigidBody.velocity != Vector2.zero)
        {
            isRolling = true;
            rollDur = totalRollDuration;
            rollDir = rigidBody.velocity;
            rollCd = totalRollCd;
            oldSpeed = speed;
            
            gameObject.layer = 12;
            updateRoll();
        }
    }
}
