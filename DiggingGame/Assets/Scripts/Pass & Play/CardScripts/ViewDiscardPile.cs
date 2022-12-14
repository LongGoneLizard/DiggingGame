/*****************************************************************************
// File Name :         ViewDiscardPile.cs
// Author :            Rudy W
// Creation Date :     November 26th, 2022
//
// Brief Description : Script that populates a grid with buttons based on
                       cards that are in the discard pile. These can be
                       pressed and will show that card in a maximized view.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewDiscardPile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private bool _onlineScene = false;
    [SerializeField] private CanvasGroup _discardPileViewCGroup;
    [SerializeField] private GameObject _discardedCardButtonPrefab;
    [SerializeField] private GameObject _maximizedDiscardedCard;

    [Header("Values")]
    [SerializeField] private Color _grassCardColor;
    [SerializeField] private Color _dirtCardColor;
    [SerializeField] private Color _stoneCardColor;
    [SerializeField] private Color _goldCardColor;
    private List<GameObject> _cardButtons = new List<GameObject>();

    [Header("Partner Scripts")]
    private CardManager _cm;
    private OnlineCardManager _ocm;

    /// <summary>
    /// Assigns CM
    /// </summary>
    private void Awake()
    {
        if(_onlineScene)
        {
            _ocm = FindObjectOfType<OnlineCardManager>();
        }
        else
        {
            _cm = FindObjectOfType<CardManager>();
        }
    }

    /// <summary>
    /// Makes the DPile View Invisible
    /// </summary>
    private void Start()
    {
        HideDiscardView();
    }

    /// <summary>
    /// Displays discarded cards based on the DPile.
    /// </summary>
    private void DisplayDiscardedCards()
    {
        List<GameObject> pileToUse = new List<GameObject>();
        if(_onlineScene)
        {
            pileToUse = _ocm.DPile;
        }
        else
        {
            pileToUse = _cm.DPile;
        }

        foreach(GameObject cardButton in _cardButtons)
        {
            Destroy(cardButton);
        }

        GameObject newCard;

        for (int i = 0; i < pileToUse.Count; i++)
        {
            newCard = Instantiate(_discardedCardButtonPrefab, transform);
            newCard.GetComponent<DiscardedCardButton>().DiscardedCard = pileToUse[i].GetComponentInChildren<CardVisuals>().ThisCard;

            if (pileToUse[i].GetComponentInChildren<CardVisuals>().ThisCard.GrassSuit)
            {
                newCard.GetComponent<DiscardedCardButton>().PrepareButton(_grassCardColor);
            }
            else if (pileToUse[i].GetComponentInChildren<CardVisuals>().ThisCard.DirtSuit)
            {
                newCard.GetComponent<DiscardedCardButton>().PrepareButton(_dirtCardColor);
            }
            else if (pileToUse[i].GetComponentInChildren<CardVisuals>().ThisCard.StoneSuit)
            {
                newCard.GetComponent<DiscardedCardButton>().PrepareButton(_stoneCardColor);
            }
            else if(pileToUse[i].GetComponentInChildren<CardVisuals>().ThisCard.GoldSuit)
            {
                newCard.GetComponent<DiscardedCardButton>().PrepareButton(_goldCardColor);
            }

            _cardButtons.Add(newCard);
        }
    }

    /// <summary>
    /// Displays the actual card.
    /// </summary>
    /// <param name="cardToShow">Card Scriptable Obj, from button</param>
    public void DisplaySpecificCard(Card cardToShow)
    {
        _maximizedDiscardedCard.SetActive(true);

        _maximizedDiscardedCard.GetComponent<CardVisuals>().ThisCard = cardToShow;
        _maximizedDiscardedCard.GetComponent<CardVisuals>().PrepareCardSuit();
        _maximizedDiscardedCard.GetComponent<CardVisuals>().PrepareCardValues();
    }

    /// <summary>
    /// Shows the discard view.
    /// </summary>
    public void ShowDiscardView()
    {
        _maximizedDiscardedCard.SetActive(false);
        DisplayDiscardedCards();

        _discardPileViewCGroup.blocksRaycasts = true;
        _discardPileViewCGroup.interactable = true;
        _discardPileViewCGroup.alpha = 1;
    }

    /// <summary>
    /// Hides the discard view.
    /// </summary>
    public void HideDiscardView()
    {
        _maximizedDiscardedCard.SetActive(false);

        _discardPileViewCGroup.interactable = false;
        _discardPileViewCGroup.blocksRaycasts = false;
        _discardPileViewCGroup.alpha = 0;
    }
}
