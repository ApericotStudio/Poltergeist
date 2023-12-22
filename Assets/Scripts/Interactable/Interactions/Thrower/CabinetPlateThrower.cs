using System.Collections;
using UnityEngine;
public class CabinetPlateThrower : Thrower
{
    private ObjectWithDoor _door;

    protected override void Awake()
    {
        base.Awake();
        _door = GetComponent<ObjectWithDoor>();
    }

    public override void Throw()
    {
        StartCoroutine(ThrowPlates());
    }

    private IEnumerator ThrowPlates()
    {
        _door.Use();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ThrowObjects(randomThrowForce: true, randomObjectPick: false, amountThrownAtATime: 1));
    }
}
