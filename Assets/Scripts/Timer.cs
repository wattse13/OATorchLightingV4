using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Attatched to the Time GameObject
// Does NOT currently work
// Causes game to freeze in current state 
//public class Timer : MonoBehaviour
//{
//    [SerializeField]
//    private int timeSpeedOriginal = 1;
//    [SerializeField]
//    private int speedVarOriginal = 1;
//    [SerializeField]
//    private float oxySpeedVar = 1;
//    [SerializeField]
//    private int oxyDirection = 1;
//    [SerializeField]
//    private float acetylSpeedVar = 1;
//    [SerializeField]
//    private int acetylDirection = 1;
//    [SerializeField]
//    private int changeDirection = -1;
//    [SerializeField]
//    private float slowDown = 1 / 4;
//    [SerializeField]
//    private int speedUp = 4;
//    [SerializeField] // Just for debugging purposes
//    private float currentOxygen;
//    [SerializeField] // Just for debugging purposes
//    private float currentAcetylene;
//    [SerializeField]
//    private float maxOxygen = 100;
//    [SerializeField]
//    private float maxAcetylene = 100;

//    private int oxyLeak = 1;
//    private int acetylLeak = 2;

//    private void OnEnable()
//    {
//        SafetyStateController.OnLeaked += StartTimer;
//        // SafetyStateController.OnStoppedLeak += StopTimer;
//    }

//    private void OnDisable()
//    {
//        SafetyStateController.OnLeaked -= StartTimer;
//        // SafetyStateController.OnStoppedLeak -= StopTimer;
//    }

//    private void StartTimer(int id, bool leak)
//    {
//        if (id == oxyLeak)
//        {
//            while (leak == true && currentOxygen < maxOxygen)
//            {
//                currentOxygen += Time.deltaTime * oxySpeedVar;
//                Debug.Log(currentOxygen);
//            }
//            while (leak == false && currentOxygen > 0)
//            {
//                currentOxygen -= Time.deltaTime; // Should add some sort of speed modifyer
//                Debug.Log(currentOxygen);
//            }
//        }
//        if (id == acetylLeak)
//        {
//            while (leak == true && currentAcetylene < maxAcetylene)
//            {
//                currentAcetylene += Time.deltaTime * acetylSpeedVar;
//                Debug.Log(currentAcetylene);
//            }
//            while (leak == false && currentAcetylene > 0)
//            {
//                currentAcetylene -= Time.deltaTime; // Should add some sort of speed modifyer
//                Debug.Log(currentAcetylene);
//            }
//        }
//    }

    //private void StopTimer(int id)
    //{
    //    if (id == oxyLeak)
    //    {
    //        while (currentOxygen > 0)
    //        {
    //            currentOxygen -= Time.deltaTime; 
    //            Debug.Log(currentOxygen);
    //        }
    //    }
    //    if (id == acetylLeak)
    //    {
    //        while (currentAcetylene > 0)
    //        {
    //            currentAcetylene -= Time.deltaTime; // Should add some sort of speed modifyer
    //        }
    //    }
    //}
//}
