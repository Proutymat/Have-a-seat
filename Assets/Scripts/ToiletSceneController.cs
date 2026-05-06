using UnityEngine;

public class ToiletSceneController : MonoBehaviour
{
    public ToiletState State { get; private set; } = ToiletState.Empty;

    public void UseToilet()
    {
        if (State == ToiletState.Empty)
            State = ToiletState.UsedToilet;
    }

    public void UsePaper()
    {
        if (State == ToiletState.UsedToilet)
            State = ToiletState.PaperInside;
    }

    public void Clean()
    {
        if (State == ToiletState.PaperInside)
            State = ToiletState.Wiped;
    }

    public void Flush()
    {
        if (State == ToiletState.Wiped)
            State = ToiletState.Finished;
    }

    public bool CanUseToilet => State == ToiletState.Empty;
    public bool CanUsePaper => State == ToiletState.UsedToilet;
    public bool CanClean => State == ToiletState.PaperInside;
    public bool CanFlush => State == ToiletState.Wiped;
}