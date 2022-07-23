using UnityEngine;

public class ScaleUpTriggerController : MonoBehaviour
{
    public PlayerScalingController scalingController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            scalingController.IsRoomToScaleUp = false;
        }
    }

    private void OnTriggerStay(Collider other) // need this to pervent false positive
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            scalingController.IsRoomToScaleUp = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            scalingController.IsRoomToScaleUp = true;
        }
    }
}
