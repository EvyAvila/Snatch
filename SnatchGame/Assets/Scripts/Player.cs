using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayerState { Idle, Move }
public class Player : Entity
{
    #region SetFieldsInHigherBase
    //public Vector3 Direction;
    //public float Speed { get; set; }
    //public float DefaultSpeed { get; set; }

    //public PlayerState playerState;
    //public float RotateSpeed { get; set; }
    #endregion
    
    public BoxCollider boxCollider { get; set; }
    public Rigidbody rigidBody { get; set; }
}
