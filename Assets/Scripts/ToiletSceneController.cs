using UnityEngine;

public class ToiletSceneController : MonoBehaviour
{
    
    public ToiletState State { get; private set; } = ToiletState.Free;
    public int PaperUsedThisPoop { get; private set; } = 0;
    public int PaperRemaining { get; private set; } = 5;
    
    
    public void ConsumePaper()
    {
        PaperUsedThisPoop += 1;
        PaperRemaining -= 1;
    }

    public void TakeBroom()
    {
        
    }

    public void PutPaperOnToilet()
    {
        State = State == ToiletState.Free ? ToiletState.Carpeted : ToiletState.VeryCarpeted;
    }
    
    public void SitOnToilet()
    {
        State = ToiletState.InUse;
    }

    public void Flush()
    {
        State = ToiletState.Dirty;
    }

    public void CleanToilet()
    {
        State = ToiletState.Cleaned;
    }

    public void UseSink()
    {
        
    }
    
}