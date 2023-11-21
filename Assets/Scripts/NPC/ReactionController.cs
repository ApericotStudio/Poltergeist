using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Reaction Controller is responsible for displaying the correct reaction sprite above the NPC's head based on the fear value of the NPC.
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
    [Tooltip("The sprite that will be displayed when the NPC is scared."), SerializeField]
    private Sprite _scaredSprite;

    private NpcController _npcController;
    private Canvas _reactionCanvas;
    private Image _reactionImage;

    private void Awake()
    {
        _reactionCanvas = GetComponentInParent<Canvas>();
        _npcController = GetComponentInParent<NpcController>();
        _npcController.OnFearValueChange.AddListener(SetNpcReactionBasedOnFear);
        _npcController.OnStateChange.AddListener(SetNpcReactionBasedOnState);
        _reactionImage = GetComponent<Image>();
    }

    private void Update()
    {
        RotateCanvasTowardsPlayer();
    }

    private void SetNpcReactionBasedOnFear(float fear)
    {
        if(_npcController.CurrentState is InvestigateState)
        {
            return;
        }
        if(_npcController.CurrentState is ScaredState)
        {
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

    private void SetNpcReactionBasedOnState()
    {
        if(_npcController.CurrentState == _npcController.InvestigateState)
        {
            _reactionImage.sprite = _investigateSprite;
        }
        else if(_npcController.CurrentState == _npcController.ScaredState)
        {
            _reactionImage.sprite = _scaredSprite;
        }
        else
        {
            SetNpcReactionBasedOnFear(_npcController.FearValue);
        }
    }

    /// <summary>
    /// Rotates the canvas towards the player.
    /// </summary>
    private void RotateCanvasTowardsPlayer()
    {
        Vector3 directionToCamera = Camera.main.transform.position - _reactionCanvas.transform.position;
        _reactionCanvas.transform.rotation = Quaternion.LookRotation(-directionToCamera);
    }
}
