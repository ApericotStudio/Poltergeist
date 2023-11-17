using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Reaction Module is responsible for displaying the correct reaction sprite above the NPC's head based on the fear value of the NPC.
/// </summary>
public class ReactionController : MonoBehaviour
{
    [Tooltip("The sprite that will be displayed when the fear value is low."), SerializeField]
    private Sprite _lowFearSprite;
    [Tooltip("The sprite that will be displayed when the fear value is medium."), SerializeField]
    private Sprite _mediumFearSprite;
    [Tooltip("The sprite that will be displayed when the fear value is high."), SerializeField]
    private Sprite _highFearSprite;
    [Tooltip("The sprite that will be displayed when the NPC is investigating."), SerializeField]
    private Sprite _investigateSprite;

    private NpcController _npcController;
    private Canvas _reactionCanvas;
    private Image _reactionImage;

    private void Awake()
    {
        _reactionCanvas = GetComponentInParent<Canvas>();
        _npcController = GetComponentInParent<NpcController>();
        _npcController.OnFearValueChange.AddListener(SetNpcReaction);
        _reactionImage = GetComponent<Image>();
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
        if(_npcController.CurrentState == _npcController.InvestigateState && fear <= 75f)
        {
            _reactionImage.sprite = _investigateSprite;
            return;
        }
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
