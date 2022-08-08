using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main class that will hold all the CheckpointSingle objects
/// </summary>
public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

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
            print(checkpointSingleTransform.name);
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

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {
        print($"carTransformList.IndexOf(carTransform): {carTransformList.IndexOf(carTransform)}");
        print($"nextCheckpointSingleIndexList.Count: {nextCheckpointSingleIndexList.Count}");
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            // Correct checkpoint
            Debug.Log("Correct");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Wrong checkpoint
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }


}
