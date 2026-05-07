using UnityEngine;

public class DesktopSceneController : MonoBehaviour
{
    
    public DesktopState State { get; private set; } = DesktopState.Working;

    
    public void ActivateLever()
    {
        
    }

    public void PauseWork()
    {
        State = DesktopState.Paused;
    }
}
