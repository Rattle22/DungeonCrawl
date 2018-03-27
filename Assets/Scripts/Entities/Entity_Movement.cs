using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Movement : MonoBehaviour {

    protected float speed = 1;
    protected Rigidbody2D rigidBody;

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void move(Vector3 direction)
    {
        direction.Normalize();
        rigidBody.velocity = direction * speed;
    }

    /// <summary>
    /// Rotates this object to align with the given Vector.
    /// </summary>
    /// <param name="newRotation"></param>
    public void rotate(Vector2 orientation)
    {
        float rotation = 0;
        rotation = Mathf.Atan2(orientation.y, orientation.x);
        rotation *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    /// <summary>
    /// Rotates this object to align with the given Vector, but only up to the given rotation.
    /// 
    /// Use this to enforce turn speeds.
    /// </summary>
    /// <param name="orientation"></param>
    /// <param name="maxRot"></param>
    public void rotate(Vector2 orientation, float maxRot)
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        float wantedRotation = 0;
        wantedRotation = Mathf.Atan2(orientation.y, orientation.x);
        wantedRotation *= Mathf.Rad2Deg;

        float rotDiff = Mathf.DeltaAngle(currentRotation, wantedRotation);
        rotDiff = (rotDiff > 0 ? 1 : -1) * Mathf.Min(maxRot, Mathf.Abs(rotDiff));

        rotate(rotDiff);
    }

    /// <summary>
    /// Rotates this object by the given amount around the z Axis.
    /// </summary>
    /// <param name="angle"></param>
    public void rotate(float angle)
    {
        float currentRot = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.AngleAxis(angle + currentRot, Vector3.forward);
    }

    protected virtual void Start()
    {
        if (rigidBody == null) {
            rigidBody = GetComponent<Rigidbody2D>();
        }
    }
}
