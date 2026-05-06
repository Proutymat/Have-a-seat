using Sirenix.OdinInspector;
using UnityEngine;

public class ToiletBroomInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    [SerializeField] private ToiletSceneController _scene;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private HandController _hand;

    public void Interact()
    {
        _scene.TakeBroom();
        _hand.HoldBroom();
    }

    public bool CanInteract()
    {
        return _scene.State == ToiletState.Dirty &&  _hand.State == HandState.Free;
    }
}
