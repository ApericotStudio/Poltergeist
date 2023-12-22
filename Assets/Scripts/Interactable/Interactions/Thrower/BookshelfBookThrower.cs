using UnityEngine;

public class Bookshelf : Thrower
{
    [Header("Bookshelf Settings Settings")]
    [Tooltip("Toggle whether the books are thrown randomly or not."), SerializeField]
    private bool _randomBooksThrown = true;
    [Tooltip("The amount of books thrown at a time."), Range(0, 10), SerializeField]
    private int _booksThrownAtATime = 3;

    private int _totalBooks;
    private Interactable _interactable;

    protected override void Awake()
    {
        base.Awake();
        _interactable = GetComponent<Interactable>();
        _totalBooks = _objectRigidbodies.Count;
    }

    public override void Throw()
    {
        int amountToThrow = (int)System.Math.Ceiling((double)_totalBooks / _interactable.MaxUses);
        StartCoroutine(ThrowObjects(amount: amountToThrow, randomThrowForce: _randomBooksThrown, amountThrownAtATime: _booksThrownAtATime));
    }
}
