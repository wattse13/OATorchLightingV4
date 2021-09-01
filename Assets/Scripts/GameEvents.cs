using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to GameEvents GameObject
// Subscribed to SafetyStateController OnConsequence Delegate Event
public class GameEvents : MonoBehaviour
{
    private int oxyLeak = 1;
    private int acetylLeak = 3;
    private int oxyBurnout = 2;
    private int acetylBurnout = 4;
    private int oxyHoseLeak = 5;
    private int acetylHoseLeak = 6;
    private int handExplosion = 7;
    private int smallExplosion = 8;
    private int bigExplosion = 9;
    private int noAcetyl = 10;

    // ModalWindow is subscribed to this delegate event
    // Used to change the text on the modal window
    public delegate void SendConsequenceMessage(string message);
    public static event SendConsequenceMessage OnResetMessageSent;
    public static event SendConsequenceMessage OnContinueMessageSent;

    private void OnEnable()
    {
        SafetyStateController.OnConsequence += WhoCalled;
    }

    private void OnDisable()
    {
        SafetyStateController.OnConsequence -= WhoCalled;
    }

    // When GameEvents recieves a message from OnClickDelegate, it triggers its own delegate event, OnMessageSent
    // Passes clicked on GameObject as reference
    // Id system is very fragile
    public void WhoCalled(int eventID)
    {
        // calls function based on which eventID is passed in to argument
        if (eventID == oxyBurnout || eventID == acetylBurnout)
        {
            BurnoutEvent(eventID);
        }
        if (eventID == oxyLeak || eventID == acetylLeak || eventID == oxyHoseLeak || eventID == acetylHoseLeak)
        {
            LeakEvent(eventID);
        }
        if (eventID == handExplosion || eventID == smallExplosion || eventID == bigExplosion || eventID == noAcetyl)
        {
            ExplosionEvent(eventID);
        }
    }

    public void BurnoutEvent(int eventID)
    {
        if (eventID == oxyBurnout)
        {
            OnResetMessageSent?.Invoke("The Oxygen Regulator Burned Out! This kind of accident is deadly. Reset to try again.");
            Debug.Log("Oxy Reg Burnout!");
        }
        if (eventID == acetylBurnout)
        {
            Debug.Log("Acetyl Reg Burnout");
            OnResetMessageSent?.Invoke("The Acetylene Regulator Burned Out! This kind of accident is deadly. Reset to try again.");
        }
    }

    public void LeakEvent(int eventID)
    {
        if (eventID == oxyLeak)
        {
            Debug.Log("oxy leak");
            OnContinueMessageSent?.Invoke("There's a hissing noise coming from the torch. It doesn't smell.");
        }
        if (eventID == acetylLeak)
        {
            Debug.Log("acetyl leak");
            OnContinueMessageSent?.Invoke("There's a hissing noise coming from the torch. It smells, kind of, garlicy?");
        }
        if (eventID == oxyHoseLeak)
        {
            Debug.Log("oxy hose leak");
            OnContinueMessageSent?.Invoke("There's a hissing noise coming from the oxygen hose. It doesn't smell.");
        }
        if (eventID == acetylHoseLeak)
        {
            Debug.Log("acetyl hose leak");
            OnContinueMessageSent?.Invoke("There's a hissing noise coming from the acetylene hose. It smells, kind of, garlicy?");
        }
    }

    public void ExplosionEvent(int eventID)
    {
        if (eventID == handExplosion)
        {
            Debug.Log("The lighter exploaded in your hand!");
            OnResetMessageSent?.Invoke("The lighter exploded in your hands! Hopefully you were wearing gloves. Reset to try again.");
        }
        if (eventID == smallExplosion)
        {
            Debug.Log("boom!");
            OnResetMessageSent?.Invoke("There's an explosion! Small amounts of gas have been leaking from improperly used equipment. Reset to try again.");
        }
        if (eventID == bigExplosion)
        {
            Debug.Log("BOOM!");
            OnResetMessageSent?.Invoke("There's a large explosion! Large amounts of gas have been leaking from improperly used equipment. Reset to try again.");
        }
        if (eventID == noAcetyl)
        {
            Debug.Log("The torch won't light");
            OnContinueMessageSent?.Invoke("Hmmm, There's definately gas coming out of the torch, but the lighter isn't igniting it.");
        }
    }
}
