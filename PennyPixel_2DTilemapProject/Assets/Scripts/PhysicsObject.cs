﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected Vector2 velocity;
    public float minimumGroundNormalY = .65f;
    public float gravityModifier = 1f;
    protected Vector2 targetVelocity;
    protected Rigidbody2D rb2d;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected const float minMoveDistance = 0.001f;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);
    
    protected const float shellRadius = 0.01f;
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }
    protected virtual void ComputeVelocity()
    {

    }
    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        // Debug.Log("Vel " + velocity + "Grav " + gravityModifier + "2DGrav " + Physics2D.gravity + "Time " + Time.deltaTime);
        velocity.x = targetVelocity.x;
        grounded = false;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y,-groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move,false);
        // Debug.Log(deltaPosition.y);
        move = Vector2.up * deltaPosition.y;
        // Debug.Log("Move2; " + move);
        Movement(move,true);
    }
    void Movement(Vector2 move,bool yMovement)
    {
        float distance = move.magnitude;
        // Debug.Log("Move;" + move);
        if(distance > minMoveDistance)
        {
            int count = rb2d.Cast(move,contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for(int i = 0; i <count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for(int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if(currentNormal.y > minimumGroundNormalY)
                {
                    grounded = true;
                    if(yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(velocity,currentNormal);
                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDist = hitBufferList[i].distance - shellRadius;
                distance = modifiedDist < distance ? modifiedDist : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
