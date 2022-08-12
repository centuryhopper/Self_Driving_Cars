using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarDriverAgent : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;
    private CarDriver carDriver;
    private Quaternion recallRotation;
    private Vector3 recallPosition;

    [System.Serializable]
    public struct RewardsInfo
    {
        public float correctCheckpoint, wrongCheckpoint;
        public float hitLastCheckpoint, hitAWall, slidingAlongWall;
        public float movingForward, movingBackwards, noMovement;
        public float notFacingCheckpoint;
    }

    [SerializeField] private RewardsInfo rwd;

    public override void Initialize()
    {
        carDriver = GetComponent<CarDriver>();
        recallRotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);

        recallPosition = spawnPosition.position + new Vector3(Random.Range(-5f,+5f), 0, Random.Range(-5f,+5f));

        if (trackCheckpoints is null)
        {
            trackCheckpoints = GameObject.Find("CheckPoints").GetComponent<TrackCheckpoints>();
        }

        trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
        trackCheckpoints.OnAgentCompleteTrack += resetAgent;
        trackCheckpoints.OnAgentCompleteTrack += rewardAgent;
    }


    private void TrackCheckpoints_OnCarCorrectCheckpoint(Transform carTransform)
    {
        if (carTransform == transform)
        {
            // print("correct checkpoint");
            AddReward(rwd.correctCheckpoint);
        }
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(Transform carTransform)
    {
        if (carTransform == transform)
        {
            // print("wrong checkpoint");
            AddReward(rwd.wrongCheckpoint);
        }
    }

    private void resetAgent()
    {
        EndEpisode();
    }
    private void rewardAgent()
    {
        AddReward(rwd.hitLastCheckpoint);
    }

    public override void OnEpisodeBegin()
    {
        // print("episode begin");
        transform.position = recallPosition;
        transform.forward = spawnPosition.forward;
        transform.rotation = recallRotation;
        trackCheckpoints.resetCheckPoint(transform);
        carDriver.StopCompletely();
    }

    private float GetDotWithNextCheckpoint()
    {
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        return directionDot;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var dot = GetDotWithNextCheckpoint();
        if (dot < 0.9f)
        {
            AddReward(rwd.notFacingCheckpoint);
        }
        sensor.AddObservation(dot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0f, turnAmount = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0:
                forwardAmount = 0f;
                AddReward(rwd.noMovement);
                break;
            case 1:
                forwardAmount = +1f;
                // encourage moving forward
                AddReward(rwd.movingForward);
                break;
            case 2:
                forwardAmount = -1f;
                AddReward(rwd.movingBackwards);
                break;
        }

        switch (actions.DiscreteActions[1])
        {
            case 0:
                turnAmount = 0f;
                break;
            case 1:
                turnAmount = +1f;
                break;
            case 2:
                turnAmount = -1f;
                break;
        }

        carDriver.SetInputs(forwardAmount, turnAmount);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
        if (Input.GetKey(KeyCode.S) ||  Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.D) ||  Input.GetKey(KeyCode.RightArrow)) turnAction = 1;
        if (Input.GetKey(KeyCode.A) ||  Input.GetKey(KeyCode.LeftArrow)) turnAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;

        print($"dot with next checkpoint: {GetDotWithNextCheckpoint()}");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            // the car has hit a wall
            // punish the ai
            if (trackCheckpoints.GetNextCheckpoint(transform).name != "CheckpointSingle (67)")
            {
                // the harder you hit the wall, the more the punish
                AddReward(rwd.hitAWall * collision.relativeVelocity.sqrMagnitude);
            }
            // EndEpisode();
            // print("ended episode");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            // the car has hit a wall
            // punish the ai
            // avoid the ai from driving the car along the wall
            AddReward(rwd.slidingAlongWall);
        }
    }

}

