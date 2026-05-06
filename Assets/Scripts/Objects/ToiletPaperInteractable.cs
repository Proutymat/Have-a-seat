using Sirenix.OdinInspector;
using UnityEngine;

public class ToiletPaperInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    [SerializeField] private ToiletSceneController _scene;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private HandController _hand;

    public void Interact()
    {
        _scene.ConsumePaper();
        _hand.HoldToiletPaper();
    }

    public bool CanInteract()
    {
        return _scene.PaperRemaining > 0 && _scene.PaperUsedThisPoop < 2 && // Must remain toilet paper
               ((_player.State == PlayerState.Free && _hand.State == HandState.Free && (_scene.State == ToiletState.Free || _scene.State == ToiletState.Carpeted)) // Before pooping to carpet toilet
                || (_player.State == PlayerState.Sitting && _hand.State == HandState.Free && _scene.State == ToiletState.Full)); // When you've pooped
    }
}