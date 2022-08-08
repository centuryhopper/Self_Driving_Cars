using UnityEngine;

public class TrackCheckpointsUI : MonoBehaviour
{

    [SerializeField] private TrackCheckpoints trackCheckpoints;

    void OnEnable()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpoints_OnPlayerCorrectCheckpoint;
        trackCheckpoints.OnPlayerWrongCheckpoint += TrackCheckpoints_OnPlayerWrongCheckpoint;
    }

    void OnDisable()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint -= TrackCheckpoints_OnPlayerCorrectCheckpoint;
        trackCheckpoints.OnPlayerWrongCheckpoint -= TrackCheckpoints_OnPlayerWrongCheckpoint;
    }

    private void Start()
    {
        Hide();
    }

    private void TrackCheckpoints_OnPlayerWrongCheckpoint()
    {
        Show();
    }

    private void TrackCheckpoints_OnPlayerCorrectCheckpoint()
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
