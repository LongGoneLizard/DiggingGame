/*****************************************************************************
// File Name :         OnlineCardController.cs
// Author :            Rudy Wolfer, Andrea Swihart-DeCoster
// Creation Date :     October 10th, 2022
//
// Brief Description : Script managing card/mouse interactivity and Activation.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;

// Edited: Andrea SD - Edited for online use
public class OnlineCardController : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Transform _mouseOverPos;
    [SerializeField] private Transform _selectedPos;
    [SerializeField] private Transform _defaultPos;
    [SerializeField] private GameObject _cardVisualToMaximize;
    [SerializeField] private GameObject _cardBody;
    [SerializeField] private GameObject _cardBackground;

    [Header("Values")]
    [SerializeField] private float _cardSlideSpeed;
    [SerializeField] private Color _cardDiscardColor;
    [SerializeField] private Color _cardDefaultColor;

    [Header("Other")]
    private OnlineCardManager _cm;
    private OnlineActionManager _am;
    private OnlineCanvasManager _gcm;
    private OnlineCardEffects _ce;
    private OnlineAudioPlayer _ap;
    private OnlinePersistentCardManager _pcm;
    [HideInInspector] public int HandPosition;
    [HideInInspector] public int PHandPosition;
    [HideInInspector] public int HeldByPlayer;
    private bool _currentlyMaximized = false;
    private GameObject _maximizedCard;
    private Transform _maximizeAnchor;
    [HideInInspector] public Vector3 NextPos;

    [Header("Selection Variables")]
    [HideInInspector] public bool CanBeSelected;
    [HideInInspector] public bool CanBeDiscarded;
    [HideInInspector] public bool CanBeActivated;
    [HideInInspector] public bool Selected;

    [Header("Activation Variables")]
    [HideInInspector] public bool MadePersistentP1;
    [HideInInspector] public bool MadePersistentP2;

    [Header("Animation")]
    [SerializeField] private Animator _cardAnimator;
    [SerializeField] private float _discardAnimWaitTime;
    private bool _gettingDiscarded;

    [SerializeField] private int _cardID;  //ASD
    [HideInInspector] bool readyToMove = false;

    /// <summary>
    /// Assigns partner scripts and the maximize anchor.
    /// </summary>
    private void Awake()
    {
        _maximizeAnchor = GameObject.FindGameObjectWithTag("MaximizeAnchor").GetComponent<Transform>();
        _cardBody.gameObject.name = GetComponentInChildren<CardVisuals>().ThisCard.CardName;
        _cm = FindObjectOfType<OnlineCardManager>();
        _am = FindObjectOfType<OnlineActionManager>();
        _gcm = FindObjectOfType<OnlineCanvasManager>();
        _pcm = FindObjectOfType<OnlinePersistentCardManager>();
        _ce = FindObjectOfType<OnlineCardEffects>();
        _ap = FindObjectOfType<OnlineAudioPlayer>();
        HeldByPlayer = 0;
    }

    /// <summary>
    /// Gets ViewID :3
    /// 
    /// Author: Andrea SD
    /// </summary>
    private void Start()
    {
        _cardID = photonView.ViewID; 
    }

    /// <summary>
    /// Returns the Card ID
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <returns> _cardID </returns>
    public int GetCardID()  { return _cardID; }

    /// <summary>
    /// Adjusts the card's visual position.
    /// </summary>
    private void FixedUpdate()
    {
        if (_gettingDiscarded)
        {
            return;
        }

        if (MadePersistentP1 || MadePersistentP2)
        {
            transform.position = _defaultPos.position;
            return;
        }

        if (Selected)
        {
            transform.position = _selectedPos.position;
            return;
        }

        if (transform.position != NextPos && readyToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPos, _cardSlideSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Effects that occur when the mouse hovers over a card.
    /// </summary>
    private void OnMouseEnter()
    {
        if (_gettingDiscarded)
        {
            return;
        }

        if (CanBeDiscarded)
        {
            _cardBackground.GetComponent<Image>().color = _cardDiscardColor;
        }

        if (MadePersistentP1 || MadePersistentP2)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MaximizeCard();
        }

        if (!Selected)
        {
            _ap.PlaySound("HoverCard", false);
        }

        NextPos = _mouseOverPos.position;
    }

    /// <summary>
    /// Effects that occur when the mouse leaves a card.
    /// </summary>
    private void OnMouseExit()
    {
        if (_currentlyMaximized)
        {
            DemaximizeCard();
        }

        if (_gettingDiscarded)
        {
            return;
        }

        if (CanBeDiscarded)
        {
            _cardBackground.GetComponent<Image>().color = _cardDefaultColor;
        }

        if (MadePersistentP1 || MadePersistentP2)
        {
            return;
        }

        NextPos = _defaultPos.position;
    }

    /// <summary>
    /// On click events with the card.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            MaximizeCard();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            DemaximizeCard();
        }

        if (_gettingDiscarded)
        {
            return;
        }

        if (CanBeDiscarded)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(ToDiscard());
            }
        }

        if (MadePersistentP1 || MadePersistentP2)
        {
            return;
        }

        if (CanBeSelected)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SelectCard();
            }
        }

        if (CanBeActivated)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ActivateCard();
            }
        }
    }


    /// <summary>
    /// Allows the card to move
    /// 
    /// Author: Andrea SD
    /// </summary>
    public void EnableReadyToMove()
    {
        readyToMove = true;
    }

    /// <summary>
    /// Selects the card.
    /// </summary>
    private void SelectCard()
    {
        _ap.PlaySound("SelectCard", false);

        if (!Selected)
        {
            _cm.SelectedCards.Add(_cardBody);
            Selected = true;
        }
        else
        {
            _cm.SelectedCards.Remove(_cardBody);
            Selected = false;
        }
    }

    /// <summary>
    /// Activates the card through cardeffects.
    /// </summary>
    private void ActivateCard()
    {
        CardVisuals cv = _cardBody.GetComponentInChildren<CardVisuals>();
        int grassCost = cv.ThisCard.GrassCost;
        int dirtCost = cv.ThisCard.DirtCost;
        int stoneCost = cv.ThisCard.StoneCost;

        if (_cm.AllowedActivations == 0)
        {
            _gcm.UpdateCurrentActionText("You've Activated the max amount of Cards.");
            FindObjectOfType<SFXManager>().PlayButtonSound();
            return;
        }

        if (_am.CurrentPlayer == 1)
        {
            if (_am.P1RefinedPile[0] >= grassCost && _am.P1RefinedPile[1] >= dirtCost && _am.P1RefinedPile[2] >= stoneCost)
            {
                StatManager.s_Instance.IncreaseStatistic(_am.CurrentPlayer, "Activation", 1);

                _am.CallUpdatePieces(1, 1, 0, -grassCost);
                _am.CallUpdatePieces(1, 1, 1, -dirtCost);
                _am.CallUpdatePieces(1, 1, 2, -stoneCost);
                _am.SupplyPileRPC(0, grassCost);
                _am.SupplyPileRPC(1, dirtCost);
                _am.SupplyPileRPC(2, stoneCost);

                if (MultiSceneData.s_WeatherOption == 0)
                {
                    CallCardWeather();
                }

                _ap.PlaySound("ActivateCard", false);

                if (cv.ThisCard.GrassSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Grass", cv.ThisCard.CardName, _cardBody));
                }
                else if (cv.ThisCard.DirtSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Dirt", cv.ThisCard.CardName, _cardBody));
                }
                else if (cv.ThisCard.StoneSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Stone", cv.ThisCard.CardName, _cardBody));
                }
                else if (cv.ThisCard.GoldSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Gold", cv.ThisCard.CardName, _cardBody));
                }
                _cm.AllowedActivations--;
                _gcm.UpdateTextBothPlayers();
                _cm.StopCardActivating(_am.CurrentPlayer);

                if (!GetComponentInChildren<CardVisuals>().ThisCard.persistent)
                {
                    StartCoroutine(ToDiscard());
                }
                else
                {
                    _cardAnimator.Play("CardDiscard");
                }
            }
            else
            {
                _gcm.UpdateCurrentActionText("Not enough Pieces to Activate this Card!");
                FindObjectOfType<SFXManager>().PlayButtonSound();
            }
        }
        else
        {
            if (_am.P2RefinedPile[0] >= grassCost && _am.P2RefinedPile[1] >= dirtCost && _am.P2RefinedPile[2] >= stoneCost)
            {
                StatManager.s_Instance.IncreaseStatistic(_am.CurrentPlayer, "Activation", 1);

                _am.CallUpdatePieces(1, 2, 0, -grassCost);
                _am.CallUpdatePieces(1, 2, 1, -dirtCost);
                _am.CallUpdatePieces(1, 2, 2, -stoneCost);
                _am.SupplyPileRPC(0, grassCost);
                _am.SupplyPileRPC(1, dirtCost);
                _am.SupplyPileRPC(2, stoneCost);

                if (MultiSceneData.s_WeatherOption == 0)
                {
                    CallCardWeather();
                }

                _ap.PlaySound("ActivateCard", false);

                if (cv.ThisCard.GrassSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Grass", cv.ThisCard.CardName, _cardBody));
                }
                else if (cv.ThisCard.DirtSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Dirt", cv.ThisCard.CardName, _cardBody));
                }
                else if (cv.ThisCard.StoneSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Stone", cv.ThisCard.CardName, _cardBody));
                }
                else if (cv.ThisCard.GoldSuit)
                {
                    _ce.StartCoroutine(_ce.ActivateCardEffect("Gold", cv.ThisCard.CardName, _cardBody));
                }
                _cm.AllowedActivations--;
                _gcm.UpdateTextBothPlayers();
                _cm.StopCardActivating(_am.CurrentPlayer);
                if (!GetComponentInChildren<CardVisuals>().ThisCard.persistent)
                {
                    StartCoroutine(ToDiscard());
                }
                else
                {
                    _cardAnimator.Play("CardDiscard");
                }
            }
            else
            {
                _gcm.UpdateCurrentActionText("Not enough Pieces to Activate this card!");
                FindObjectOfType<SFXManager>().PlayButtonSound();
            }
        }
        _gcm.UpdateTextBothPlayers();
    }

    /// <summary>
    /// Discards the card.
    /// </summary>
    public IEnumerator ToDiscard()
    {
        _cardBackground.GetComponent<Image>().color = _cardDefaultColor;

        if (!MadePersistentP1 && !MadePersistentP2)
        { 
            if (HeldByPlayer == 1)
            {
                if (_cardBody.CompareTag("Card"))
                {
                    _cm.CallNormalCards(1, -1);
                }
                else if (_cardBody.CompareTag("GoldCard"))
                {
                    _cm.CallGoldCards(1, -1);
                }
                _cm.P1OpenHandPositions[HandPosition] = true;
                CallRemoveCard(1);   // Andrea SD
            }
            else if (HeldByPlayer == 2)
            {
                if (_cardBody.CompareTag("Card"))
                {
                    _cm.CallNormalCards(2, -1);
                }
                else if (_cardBody.CompareTag("GoldCard"))
                {
                    _cm.CallGoldCards(2, -1);
                }
                _cm.P2OpenHandPositions[HandPosition] = true;
                CallRemoveCard(2);   // Andrea SD
            }

            StatManager.s_Instance.IncreaseStatistic(_am.CurrentPlayer, "Card", 1);

            HeldByPlayer = 0;
            Selected = false;
            CanBeSelected = false;
            CanBeDiscarded = false;
            CanBeActivated = false;
            CallDiscardRPC();

            _cm.CallPileText();
        }
        else if (MadePersistentP1 || MadePersistentP2)
        {
            if (MadePersistentP1)
            {
                _pcm.P1OpenPCardSlots[PHandPosition] = true;
                CallPersistentRemoval(1);
            }
            else
            {
                _pcm.P2OpenPCardSlots[PHandPosition] = true;
                CallPersistentRemoval(2);
            }

            StatManager.s_Instance.IncreaseStatistic(_am.CurrentPlayer, "Card", 1);

            HeldByPlayer = 0;
            Selected = false;
            CanBeSelected = false;
            CanBeDiscarded = false;
            CanBeActivated = false;
            MadePersistentP1 = false;
            MadePersistentP2 = false;
            CallDiscardedPC(true);
            CallDiscardRPC();

            _cm.CallPileText();
        }

        if (_currentlyMaximized)
        {
            Destroy(_maximizedCard);
            _currentlyMaximized = false;
        }

        _cardAnimator.Play("CardDiscard");
        _ap.PlaySound("DiscardCard", false);
        _gettingDiscarded = true;
        yield return new WaitForSeconds(_discardAnimWaitTime);
        _gettingDiscarded = false;
        CallCardActive(false);
    }

    /// <summary>
    /// Maximizes a card for easier view.
    /// </summary>
    /// <param name="thingToMaximize">Card zone to maximize</param>
    private void MaximizeCard()
    {
        if (_currentlyMaximized)
        {
            return;
        }

        _ap.PlaySound("SelectCard", false);

        _maximizedCard = Instantiate(_cardVisualToMaximize, _maximizeAnchor);
        _maximizedCard.transform.position = _maximizeAnchor.transform.position;
        _currentlyMaximized = true;
    }

    /// <summary>
    /// Demaximizes a card.
    /// </summary>
    private void DemaximizeCard()
    {
        if (!_currentlyMaximized)
        {
            return;
        }

        _ap.PlaySound("SelectCard", false);

        Destroy(_maximizedCard);
        _currentlyMaximized = false;
    }

    #region RPC Function

    /// <summary>
    /// Calls the AddToDiscarded RPC across all clients. This adds the card to
    /// the discard pile.
    /// 
    /// Author: Andrea SD
    /// </summary>
    public void CallDiscardRPC()
    {
        photonView.RPC("AddToDiscarded", RpcTarget.All);
    }

    /// <summary>
    /// Adds the card to the discard pile
    /// 
    /// Author: Andrea SD
    /// </summary>
    [PunRPC]
    public void AddToDiscarded()
    {
        _cm.DPile.Add(_cardBody);
    }

    /// <summary>
    /// Calls the RPC that removes a card from player's hand
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <param name="player"></param>
    public void CallRemoveCard(int player)
    {
        photonView.RPC("RemoveCard", RpcTarget.All, player);
    }

    /// <summary>
    /// Removes a card from player's hand
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <param name="player"></param>
    [PunRPC]
    public void RemoveCard(int player)
    {
        switch(player)
        {
            case 1:
                _cm.P1Hand.Remove(_cardBody);
                break;
            case 2:
                _cm.P2Hand.Remove(_cardBody);
                break;
        }
    }

    /// <summary>
    /// Calls the RemoveFromPersistent RPC which removes a card from the
    /// persistent card area and list.
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <param name="player"> player who owns the card (1 or 2) </param>
    private void CallPersistentRemoval(int player)
    {
        photonView.RPC("RemoveFromPersistent", RpcTarget.All, player);
    }

    /// <summary>
    /// Removes a card from the persistent card area and list
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <param name="player"> player who owns the card (1 or 2) </param>
    [PunRPC]
    public void RemoveFromPersistent(int player)
    {
        switch (player)
        {
            case 1:
                _pcm.P1OpenPCardSlots[PHandPosition] = true;
                _pcm.P1PersistentCards.Remove(_cardBody);
                break;
            case 2:
                _pcm.P2OpenPCardSlots[PHandPosition] = true;
                _pcm.P2PersistentCards.Remove(_cardBody);
                break;
        }     
    }

    /// <summary>
    /// Calls the RPC that sets the weather to the activated card
    /// 
    /// Author: Andrea SD
    /// </summary>
    public void CallCardWeather()
    {
        photonView.RPC("SetCardWeather", RpcTarget.All);
    }

    /// <summary>
    /// Sets the weather to the activated card
    /// 
    /// Author: Andrea SD
    /// </summary>
    [PunRPC]
    public void SetCardWeather()
    {
        CardVisuals cv = _cardBody.GetComponentInChildren<CardVisuals>();
        FindObjectOfType<WeatherManager>().SetActiveWeather(cv.ThisCard.ChangeWeatherTo);
    }

    /// <summary>
    /// Calls the RPC that sets a card body to isActive
    /// 
    /// AUthor: Andrea SD
    /// </summary>
    /// <param name="isActive"> is the card body is active or not </param>
    public void CallCardActive(bool isActive)
    {
        photonView.RPC("SetCardActive", RpcTarget.All, isActive);
    }

    /// <summary>
    /// Sets a card body to isActive
    /// 
    /// AUthor: Andrea SD
    /// </summary>
    /// <param name="isActive"> is the card body is active or not </param>
    [PunRPC]
    public void SetCardActive(bool isActive)
    {
        _cardBody.SetActive(isActive);
    }

    /// <summary>
    /// Calls the RPC that sets the value of PCM Discarded Persistent Card
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <param name="value"> T if discarded </param>
    public void CallDiscardedPC(bool value)
    {
        photonView.RPC("SetDiscardedPC", RpcTarget.Others, value);
    }

    /// <summary>
    /// Sets the value of PCM Discarded Persistent Card
    /// 
    /// Author: Andrea SD
    /// </summary>
    /// <param name="value"> T if discarded </param>
    [PunRPC]
    public void SetDiscardedPC(bool value)
    {
        _pcm.DiscardedPersistentCard = value;
    }

    #endregion
}
