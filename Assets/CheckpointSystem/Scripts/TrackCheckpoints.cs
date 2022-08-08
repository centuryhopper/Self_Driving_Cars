using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main class that will hold all the CheckpointSingle objects
/// </summary>
public class TrackCheckpoints : MonoBehaviour
{
    public event Action OnPlayerCorrectCheckpoint, OnPlayerWrongCheckpoint;
    public event Action<Transform> OnCarCorrectCheckpoint, OnCarWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;

    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    private void Awake()
    {
        string objNameToFind = "CheckPoints";
        Transform checkpointsTransform = GameObject.Find(objNameToFind)?.transform;

        if (checkpointsTransform is null)
        {
            UnityEngine.Debug.LogError($"couldn't find the gameobject with the name: {objNameToFind}");
            return;
        }

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            // print(checkpointSingleTransform.name);
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();

        if (carTransformList is null || carTransformList.Count == 0)
        {
            var cars = GameObject.Find("Cars")?.transform;
            foreach (var car in cars)
            {
                carTransformList.Add(car as Transform);
            }
        }
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void resetCheckPoint(Transform carTransform)
    {
        int properCarIndex = carTransformList.IndexOf(carTransform);
        nextCheckpointSingleIndexList[properCarIndex] = 0;
    }

    public CheckpointSingle GetNextCheckpoint(Transform carTransform)
    {
        int properCarIndex = carTransformList.IndexOf(carTransform);
        return checkpointSingleList[properCarIndex];
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {
        // print($"carTransformList.IndexOf(carTransform): {carTransformList.IndexOf(carTransform)}");
        // print($"nextCheckpointSingleIndexList.Count: {nextCheckpointSingleIndexList.Count}");

        int properCarIndex = carTransformList.IndexOf(carTransform);
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[properCarIndex];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            // Correct checkpoint
            Debug.Log("Correct");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[properCarIndex]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke();
            OnCarCorrectCheckpoint?.Invoke(carTransform);
        }
        else
        {
            // Wrong checkpoint
            Debug.Log("Wrong");
            OnCarWrongCheckpoint?.Invoke(carTransform);
            OnPlayerWrongCheckpoint?.Invoke();

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }


}
