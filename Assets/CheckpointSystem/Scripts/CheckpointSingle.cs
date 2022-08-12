using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Hide();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarDriverAgent>(out CarDriverAgent carDriver))
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
            // print($"{other.transform.name} triggered {transform.name}");
        }
    }

    // private void OnTriggerEnter2D(Collider2D collider) {
    //     if (collider.TryGetComponent<Player_RollSpeed>(out Player_RollSpeed player)) {
    //         trackCheckpoints.CarThroughCheckpoint(this, collider.transform);
    //     }
    // }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }

}
