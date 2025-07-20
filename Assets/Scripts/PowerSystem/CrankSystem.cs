using System;
using UnityEngine;
using UnityEngine.Events;

public class CrankSystem : MonoBehaviour, IPowerProducer, IInteractable, ICursorHint, IUpgradeable
{
    private int produce = 1;
    public int Produce => produce;
    public PowerStorage PowerStorage;
    public CostObject costObject;
    public Transform Crank;

    public CursorType GetCursorType() => CursorType.Interact;
    public CostObject CostObject() => costObject;
    public UnityEvent CrankSoundEvent;

    public float rotationSpeed = 360f;
    public float lingerTime = 1f;
    private bool isSpinning = false;
    private float spinTimer = 0f;

    public void Interact()
    {
        Spin();
        PowerStorage.Store(Produce);
        CrankSoundEvent?.Invoke();
    }

    private void Awake()
    {
        if (CrankSoundEvent == null)
        {
            CrankSoundEvent = new UnityEvent();
        }
    }

    public void UpgradeLevel(int val)
    {
        produce += val;
    }

    void Update()
    {
        if (isSpinning)
        {
            Crank.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            spinTimer -= Time.deltaTime;

            if (spinTimer <= 0f)
            {
                isSpinning = false;
            }
        }
    }

    public void Spin()
    {
        spinTimer = lingerTime;

        if (!isSpinning)
        {
            isSpinning = true;
        }
    }
}
