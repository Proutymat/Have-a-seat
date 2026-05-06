using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class ToiletInteractable : MonoBehaviour, IInteractable
{
    [Title("References")]
    [SerializeField] private ToiletSceneController _scene;
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private HandController _hand;
    [SerializeField] private CinemachineCamera _toiletCamera;

    public void Interact()
    {
        // Carpet the toilet (holding paper)
        if (_hand.State == HandState.ToiletPaper)
        {
            _scene.PutPaperOnToilet();
            _hand.DropObject();
        }
        // Hand is free
        if (_hand.State == HandState.Free)
        {
            // Sit on toilet
            if (_scene.State == ToiletState.Free)
            {
                _player.SitOnToilet(_toiletCamera);
                _scene.SitOnToilet();
            }
            // Flush the toilet
            else if (_scene.State == ToiletState.Full)
            {
                _scene.Flush();
            }
        }
        // Brush the toilet
        else if (_scene.State == ToiletState.Dirty && _hand.State == HandState.ToiletBroom)
        {
            _scene.CleanToilet();
            _hand.DropObject();
        }
    }

    public bool CanInteract()
    {
        return _player.State == PlayerState.Free && 
               (_hand.State == HandState.ToiletPaper || _hand.State == HandState.ToiletBroom || _hand.State == HandState.Free) &&
               (_scene.State ==  ToiletState.Free || _scene.State == ToiletState.Carpeted || _scene.State == ToiletState.VeryCarpeted || _scene.State == ToiletState.Full);
    }
}
