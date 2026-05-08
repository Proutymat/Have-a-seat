using UnityEngine;

public class ToiletSceneController : MonoBehaviour
{
    
    public ToiletState State { get; set; } = ToiletState.Free;
    public int PaperUsedThisPoop { get; set; } = 0;
    public int PaperRemaining { get; private set; } = 5;
    
    
    public void ConsumePaper()
    {
        PaperUsedThisPoop += 1;
        PaperRemaining -= 1;
    }

    public void TakeBroom()
    {
        Debug.Log("ToiletState = "  + State);
    }

    public void PutPaperOnToilet()
    {
        State = State == ToiletState.Free ? ToiletState.Carpeted : ToiletState.VeryCarpeted;
        Debug.Log("ToiletState = "  + State);

    }
    
    public void SitOnToilet()
    {
        State = ToiletState.InUse;
        Debug.Log("ToiletState = "  + State);

    }

    public void Flush()
    {
        State = ToiletState.Dirty;
        Debug.Log("ToiletState = "  + State);

    }

    public void CleanToilet()
    {
        State = ToiletState.Cleaned;
        Debug.Log("ToiletState = "  + State);

    }

    public void UseSink()
    {
        
    }
    
}