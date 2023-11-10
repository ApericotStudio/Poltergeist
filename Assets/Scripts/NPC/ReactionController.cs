using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Reaction Module is responsible for displaying the correct reaction sprite above the NPC's head based on the fear value of the NPC.
/// </summary>
public class ReactionController : MonoBehaviour
{
    [Tooltip("The image object that will be used to display the reaction sprite."), SerializeField]
    private Image _reactionImage;
    [Tooltip("The sprite that will be displayed when the fear value is low."), SerializeField]
    private Sprite _lowFearSprite;
    [Tooltip("The sprite that will be displayed when the fear value is medium."), SerializeField]
    private Sprite _mediumFearSprite;
    [Tooltip("The sprite that will be displayed when the fear value is high."), SerializeField]
    private Sprite _highFearSprite;

    private NpcController _npcController;
    private Canvas _reactionCanvas;

    private void Awake()
    {
        _reactionCanvas = GetComponent<Canvas>();
        _npcController = GetComponentInParent<NpcController>();
        _npcController.OnFearValueChange.AddListener(SetNpcReaction);
    }

    private void Update()
    {
        _reactionCanvas.transform.LookAt(Camera.main.transform);
    }

    /// <summary>
    /// Sets the correct reaction sprite based on the fear value of the NPC.
    /// </summary>
    /// <param name="fear">The current fear value of the NPC.</param>
    private void SetNpcReaction(float fear)
    {
        if(fear <= 25f)
        {
            _reactionImage.sprite = _lowFearSprite;
        }
        else if(fear >= 75f)
        {
            _reactionImage.sprite = _highFearSprite;
        }
        else
        {
            _reactionImage.sprite = _mediumFearSprite;
        }
    }
}
