/*****************************************************************************
// File Name :         BoardManager.cs
// Author :            Rudy W.
// Creation Date :     October 7th, 2022
//
// Brief Description : Script to manage unique board-centric methods.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BoardManager : MonoBehaviour
{
    private List<GameObject> _boardPieces = new List<GameObject>();
    private ActionManager _am;

    private void Awake()
    {
        _am = FindObjectOfType<ActionManager>();
    }

    /// <summary>
    /// Calls FindBoardPieces.
    /// </summary>
    private void Start()
    {
        FindBoardPieces();
    }

    /// <summary>
    /// Adds every Board Piece to a List.
    /// </summary>
    private void FindBoardPieces()
    {
        foreach(GameObject piece in GameObject.FindGameObjectsWithTag("BoardPiece"))
        {
            _boardPieces.Add(piece);
        }
    }

    /// <summary>
    /// Sets a specified collider to be active.
    /// </summary>
    /// <param name="colliderType">"Board" "Pawn" or "Building"</param>
    public void SetActiveCollider(string colliderType)
    {
        if(colliderType == "Board")
        {
            foreach (GameObject piece in _boardPieces)
            {
                piece.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (GameObject building in GameObject.FindGameObjectsWithTag("Building"))
            {
                building.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (GameObject pawn in GameObject.FindGameObjectsWithTag("Pawn"))
            {
                pawn.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if(colliderType == "Pawn")
        {
            foreach (GameObject pawn in GameObject.FindGameObjectsWithTag("Pawn"))
            {
                pawn.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (GameObject piece in _boardPieces)
            {
                piece.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (GameObject building in GameObject.FindGameObjectsWithTag("Building"))
            {
                building.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if(colliderType == "Building")
        {
            foreach (GameObject building in GameObject.FindGameObjectsWithTag("Building"))
            {
                building.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (GameObject pawn in GameObject.FindGameObjectsWithTag("Pawn"))
            {
                pawn.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (GameObject piece in _boardPieces)
            {
                piece.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    /// <summary>
    /// Disables pawn checks with the board.
    /// </summary>
    public void DisableAllBoardInteractions()
    {
        foreach (GameObject pawn in GameObject.FindGameObjectsWithTag("Pawn"))
        {
            if (pawn.GetComponent<PlayerPawn>().PawnPlayer == _am.CurrentPlayer)
            {
                pawn.GetComponent<PlayerPawn>().UnassignAdjacentTiles();
            }
        }

        foreach (GameObject piece in _boardPieces)
        {
            piece.GetComponent<PieceController>().ShowHideBuildable(false);
            piece.GetComponent<PieceController>().ShowHideDiggable(false);
            piece.GetComponent<PieceController>().ShowHideMovable(false);
            piece.GetComponent<PieceController>().ShowHidePlaceable(false);
            piece.GetComponent<PieceController>().ShowHideEarthquake(false);
            piece.GetComponent<PieceController>().ShowHideFlippable(false);
            piece.GetComponent<PieceController>().ShowHideWalkway(false);
            piece.GetComponent<PieceController>().PieceIsSelected = false;
        }
    }

    /// <summary>
    /// Finds adjacent tiles & the tile the player is on.
    /// 1. Finds the distance between the current tile and closest other tile. 
    /// 2. Finds each tile matching that distance.
    /// 3. Adds every tile to _adjacentPieces.
    /// 4. Calls "AdjacentToPlayer" for each Piece.
    /// </summary>
    public List<GameObject> GenerateAdjacentPieceList(GameObject centralPiece)
    {
        List<GameObject> _adjacentPieces = new List<GameObject>();
        GameObject closestPiece = null;
        float curShortestDist = Mathf.Infinity;
        Vector3 curPiecePos = centralPiece.transform.position;

        foreach (GameObject piece in _boardPieces)
        {
            float pieceToPieceDist = Vector3.Distance(piece.transform.position, curPiecePos);
            if (pieceToPieceDist < curShortestDist)
            {
                closestPiece = piece;

                if (closestPiece.transform.position != curPiecePos)
                {
                    curShortestDist = pieceToPieceDist;
                }
            }
        }

        int i = 0;
        foreach (GameObject piece in _boardPieces)
        {
            if (Vector3.Distance(piece.transform.position, curPiecePos) == curShortestDist)
            {
                i++;
                _adjacentPieces.Add(piece);
            }
        }

        return _adjacentPieces;
    }
}
