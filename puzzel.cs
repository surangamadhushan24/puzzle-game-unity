
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PuzzleManager  MonoBehaviour
{
    [SerializeField] private XRSocketInteractor slot1, slot2, slot3;
    [SerializeField] private string correctTagSlot1 = Red, correctTagSlot2 = Green, correctTagSlot3 = Blue;
    [SerializeField] private UnityEvent OnSolved, OnBroken;

    private bool solved;           last known state

    void OnEnable()
    {
        slot1.selectEntered.AddListener(_ = Check()); slot1.selectExited.AddListener(_ = Check());
        slot2.selectEntered.AddListener(_ = Check()); slot2.selectExited.AddListener(_ = Check());
        slot3.selectEntered.AddListener(_ = Check()); slot3.selectExited.RemoveAllListeners();
    }

    void OnDisable()
    {
        slot1.selectEntered.RemoveAllListeners(); slot1.selectExited.RemoveAllListeners();
        slot2.selectEntered.RemoveAllListeners(); slot2.selectExited.RemoveAllListeners();
        slot3.selectEntered.RemoveAllListeners(); slot3.selectExited.RemoveAllListeners();
    }

    void Check()
    {
        bool full = slot1.hasSelection && slot2.hasSelection && slot3.hasSelection;
        bool nowSolved = false;

        if (full)
        {
            string t1 = slot1.GetOldestInteractableSelected().transform.tag;
            string t2 = slot2.GetOldestInteractableSelected().transform.tag;
            string t3 = slot3.GetOldestInteractableSelected().transform.tag;
            nowSolved = t1 == correctTagSlot1 && t2 == correctTagSlot2 && t3 == correctTagSlot3;
        }

         fire events only on state change 
        if (nowSolved && !solved) { solved = true; Debug.Log(Solved); OnSolved.Invoke(); }
        if (!nowSolved && solved) { solved = false; Debug.Log(Broken); OnBroken.Invoke(); }

         always report first wrong attempt while full 
        if (full && !nowSolved && !solved) Debug.Log(Wrong order);
    }
}