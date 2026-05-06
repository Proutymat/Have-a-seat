using UnityEngine;
using Unity.Cinemachine;

public class HandController : MonoBehaviour
{
    public HandState State { get; private set; } = HandState.Free;
    public HandDirtiness Dirtiness { get; private set; } = HandDirtiness.Clean;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInteraction _playerInteraction;

    public void HoldToiletPaper()
    {
        State = HandState.ToiletPaper;
    }

    public void HoldBroom()
    {
        State = HandState.ToiletBroom;
    }

    public void DropObject()
    {
        State = HandState.Free;
    }

    public void CleanHand()
    {
        Dirtiness = HandDirtiness.Clean;
    }

}