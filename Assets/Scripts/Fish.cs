using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The Fish class will attach to each fish and make it swim.
/// Unity doesn’t have water physics built in, so our code just moves
/// them in a straight line toward a target destination to keep things simple.
/// </summary>
public class Fish : MonoBehaviour
{

    /// <summary>
    /// controls the average speed of the fish.
    /// </summary>
    [Tooltip("The swim speed")]
    public float fishSpeed;

    /// <summary>
    /// a slightly altered speed that we will change randomly each time a new swim destination is picked.
    /// </summary>
    private float randomizedSpeed = 0f;

    /// <summary>
    /// used to trigger the selection of a new swim destination.
    /// </summary>
    private float nextActionTime = -1f;

    /// <summary>
    /// the position of the destination the fish is swimming toward.
    /// </summary>
    private Vector3 targetPosition;

    /// <summary>
    /// FixedUpdate is called at a regular interval of 0.02 seconds
    /// (it is independent of frame rate) and will allow us to interact
    /// even when the agent is training at an increased game speed,
    /// which is common for training ML-Agents. In it, we check if the
    /// fish should swim and, if so, call the Swim() function.
    /// </summary>
    void FixedUpdate()
    {
        if (fishSpeed > 0f)
        {
            Swim();
        }
    }

    /// <summary>
    /// Swim between random positions
    /// </summary>
    private void Swim()
    {
        // If it's time for the next action, pick a new speed and destination
        // Else, swim toward the destination
        if (Time.fixedTime >= nextActionTime)
        {
            // Randomize the speed
            randomizedSpeed = fishSpeed * UnityEngine.Random.Range(.5f, 1.5f);

            // Pick a random target
            targetPosition = PenguinArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);

            // Rotate toward the target
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

            // Calculate the time to get there
            float timeToGetThere = Vector3.Distance(transform.position, targetPosition) / randomizedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            // Make sure that the fish does not swim past the target
            Vector3 moveVector = randomizedSpeed * transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(transform.position, targetPosition))
            {
                transform.position += moveVector;
            }
            else
            {
                transform.position = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }
}
