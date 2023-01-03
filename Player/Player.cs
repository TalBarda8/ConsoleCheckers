using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    private readonly string m_PlayerName;        // if computer the name will be "Computer"
    private readonly char m_PieceShape;          // X or O
    private int m_Points;
    private List<string> m_ListOfRegularPieces;
    private List<string> m_ListOfKingPieces;
    private List<string> m_ListOfNonEatingValidMoves;
    private List<string> m_ListOfEatingValidMoves;

    public Player(string i_PlayerName, char i_PieceShape)
    {
        m_PlayerName = i_PlayerName;
        m_PieceShape = i_PieceShape;
        m_Points = 0;
        m_ListOfRegularPieces = new List<string>();
        m_ListOfKingPieces = new List<string>();
        m_ListOfNonEatingValidMoves = new List<string>();
        m_ListOfEatingValidMoves = new List<string>();
    }

    public int Points
    {
        get
        {
            return m_Points;
        }
        set
        {
            m_Points = value;
        }
    }

    public string Name
    {
        get
        {
            return m_PlayerName;
        }
    }

    public char Shape
    {
        get
        {
            return m_PieceShape;
        }
    }

    public List<string> RegularPieces
    {
        get
        {
            return m_ListOfRegularPieces;
        }
    }

    public List<string> KingPieces
    {
        get
        {
            return m_ListOfKingPieces;
        }
    }

    public List<string> NonEatingValidMoves
    {
        get
        {
            return m_ListOfNonEatingValidMoves;
        }
    }

    public List<string> EatingValidMoves
    {
        get
        {
            return m_ListOfEatingValidMoves;
        }
    }

    private void setListOfRegularPieces(Board i_Board)
    {
        List<string> listOfPieces = new List<string>();
        char[,] boardMatrix = i_Board.GetBoardMatrix();

        for (int colIndex = 0; colIndex < boardMatrix.GetLength(1); colIndex++)
        {
            for (int rowIndex = 0; rowIndex < boardMatrix.GetLength(0); rowIndex++)
            {
                if (boardMatrix[rowIndex, colIndex] == m_PieceShape)
                {
                    listOfPieces.Add(i_Board.GetCharLocationByIndexes(colIndex, rowIndex));
                }
            }
        }

        m_ListOfRegularPieces = listOfPieces;
    }

    private void setListOfKingPieces(Board i_Board)
    {
        List<string> listOfPieces = new List<string>();
        char[,] boardMatrix = i_Board.GetBoardMatrix();
        char kingShape;

        if (m_PieceShape == 'X')
        {
            kingShape = 'Z';
        }
        else
        {
            kingShape = 'Q';
        }

        for (int colIndex = 0; colIndex < boardMatrix.GetLength(1); colIndex++)
        {
            for (int rowIndex = 0; rowIndex < boardMatrix.GetLength(0); rowIndex++)
            {
                if (boardMatrix[rowIndex, colIndex] == kingShape)
                {
                    listOfPieces.Add(i_Board.GetCharLocationByIndexes(colIndex, rowIndex));
                }
            }
        }

        m_ListOfKingPieces = listOfPieces;
    }

    private void setListOfNonEatingValidMoves(Board i_Board)
    {
        List<string> listOfMoves = new List<string>();

        foreach (string piece in m_ListOfRegularPieces)
        {
            listOfMoves.AddRange(setListOfNonEatingValidMovesForRegularPieces(i_Board, piece));
        }

        foreach (string king in m_ListOfKingPieces)
        {
            listOfMoves.AddRange(setListOfNonEatingValidMovesForKingPieces(i_Board, king));
        }

        m_ListOfNonEatingValidMoves = listOfMoves;
    }

    private List<string> setListOfNonEatingValidMovesForRegularPieces(Board i_Board, string i_CurrentPieceLocation)
    {
        List<string> listOfMoves = new List<string>();
        char[,] boardMatrix = i_Board.GetBoardMatrix();
        int col = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(0, 1)));
        int row = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(1, 1)));
        int rowDirection;

        if (boardMatrix[row, col] == 'X')
        {
            rowDirection = -1;
        }
        else
        {
            rowDirection = 1;
        }

        if (i_Board.IsValidIndex(col + 1, row + rowDirection))
        {
            if (boardMatrix[row + rowDirection, col + 1] == ' ')
            {
                StringBuilder move = new StringBuilder();
                move.Append(i_CurrentPieceLocation);
                move.Append('>');
                move.Append(i_Board.GetCharLocationByIndexes(col + 1, row + rowDirection));
                listOfMoves.Add(move.ToString());
            }
        }

        if (i_Board.IsValidIndex(col - 1, row + rowDirection))
        {
            if (boardMatrix[row + rowDirection, col - 1] == ' ')
            {
                StringBuilder move = new StringBuilder();
                move.Append(i_CurrentPieceLocation);
                move.Append('>');
                move.Append(i_Board.GetCharLocationByIndexes(col - 1, row + rowDirection));
                listOfMoves.Add(move.ToString());
            }
        }

        return listOfMoves;
    }

    private List<string> setListOfNonEatingValidMovesForKingPieces(Board i_Board, string i_CurrentPieceLocation)
    {
        List<string> listOfMoves = new List<string>();
        char[,] boardMatrix = i_Board.GetBoardMatrix();
        int col = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(0, 1)));
        int row = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(1, 1)));

        if (i_Board.IsValidIndex(col + 1, row + 1))
        {
            if (boardMatrix[row + 1, col + 1] == ' ')
            {
                StringBuilder move = new StringBuilder();
                move.Append(i_CurrentPieceLocation);
                move.Append('>');
                move.Append(i_Board.GetCharLocationByIndexes(col + 1, row + 1));
                listOfMoves.Add(move.ToString());
            }
        }

        if (i_Board.IsValidIndex(col - 1, row + 1))
        {
            if (boardMatrix[row + 1, col - 1] == ' ')
            {
                StringBuilder move = new StringBuilder();
                move.Append(i_CurrentPieceLocation);
                move.Append('>');
                move.Append(i_Board.GetCharLocationByIndexes(col - 1, row + 1));
                listOfMoves.Add(move.ToString());
            }
        }

        if (i_Board.IsValidIndex(col + 1, row - 1))
        {
            if (boardMatrix[row - 1, col + 1] == ' ')
            {
                StringBuilder move = new StringBuilder();
                move.Append(i_CurrentPieceLocation);
                move.Append('>');
                move.Append(i_Board.GetCharLocationByIndexes(col + 1, row - 1));
                listOfMoves.Add(move.ToString());
            }
        }

        if (i_Board.IsValidIndex(col - 1, row - 1))
        {
            if (boardMatrix[row - 1, col - 1] == ' ')
            {
                StringBuilder move = new StringBuilder();
                move.Append(i_CurrentPieceLocation);
                move.Append('>');
                move.Append(i_Board.GetCharLocationByIndexes(col - 1, row - 1));
                listOfMoves.Add(move.ToString());
            }
        }

        return listOfMoves;
    }

    private void setListOfEatingValidMoves(Board i_Board)
    {
        List<string> listOfMoves = new List<string>();

        foreach (string piece in m_ListOfRegularPieces)
        {
            listOfMoves.AddRange(setListOfEatingValidMovesForRegularPieces(i_Board, piece));
        }

        foreach (string king in m_ListOfKingPieces)
        {
            listOfMoves.AddRange(setListOfEatingValidMovesForKingPieces(i_Board, king));
        }

        m_ListOfEatingValidMoves = listOfMoves;
    }

    private List<string> setListOfEatingValidMovesForRegularPieces(Board i_Board, string i_CurrentPieceLocation)
    {
        List<string> listOfMoves = new List<string>();
        char[,] boardMatrix = i_Board.GetBoardMatrix();
        int col = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(0, 1)));
        int row = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(1, 1)));
        char opponentPieceRegularShape;
        char opponentPieceKingShape;
        int rowDirection;

        if (boardMatrix[row, col] == 'X')
        {
            rowDirection = -1;
            opponentPieceRegularShape = 'O';
            opponentPieceKingShape = 'Q';
        }
        else
        {
            rowDirection = 1;
            opponentPieceRegularShape = 'X';
            opponentPieceKingShape = 'Z';
        }

        if (i_Board.IsValidIndex(col + 2, row + (2 * rowDirection)))
        {
            if (boardMatrix[row + rowDirection, col + 1] == opponentPieceRegularShape ||
                boardMatrix[row + rowDirection, col + 1] == opponentPieceKingShape)
            {
                if (boardMatrix[row + (2 * rowDirection), col + 2] == ' ')
                {
                    StringBuilder move = new StringBuilder();
                    move.Append(i_CurrentPieceLocation);
                    move.Append('>');
                    move.Append(i_Board.GetCharLocationByIndexes(col + 2, row + (2 * rowDirection)));
                    listOfMoves.Add(move.ToString());
                }
            }
        }

        if (i_Board.IsValidIndex(col - 2, row + (2 * rowDirection)))
        {
            if (boardMatrix[row + rowDirection, col - 1] == opponentPieceRegularShape ||
                boardMatrix[row + rowDirection, col - 1] == opponentPieceKingShape)
            {
                if (boardMatrix[row + (2 * rowDirection), col - 2] == ' ')
                {
                    StringBuilder move = new StringBuilder();
                    move.Append(i_CurrentPieceLocation);
                    move.Append('>');
                    move.Append(i_Board.GetCharLocationByIndexes(col - 2, row + (2 * rowDirection)));
                    listOfMoves.Add(move.ToString());
                }
            }
        }

        return listOfMoves;
    }

    private List<string> setListOfEatingValidMovesForKingPieces(Board i_Board, string i_CurrentPieceLocation)
    {
        List<string> listOfMoves = new List<string>();
        char[,] boardMatrix = i_Board.GetBoardMatrix();
        int col = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(0, 1)));
        int row = i_Board.GetIndexByChar(char.Parse(i_CurrentPieceLocation.Substring(1, 1)));
        char opponentPieceRegularShape;
        char opponentPieceKingShape;

        if (boardMatrix[row, col] == 'Z')
        {
            opponentPieceRegularShape = 'O';
            opponentPieceKingShape = 'Q';
        }
        else
        {
            opponentPieceRegularShape = 'X';
            opponentPieceKingShape = 'Z';
        }

        if (i_Board.IsValidIndex(col + 2, row + 2))
        {
            if (boardMatrix[row + 1, col + 1] == opponentPieceRegularShape ||
                boardMatrix[row + 1, col + 1] == opponentPieceKingShape)
            {
                if (boardMatrix[row + 2, col + 2] == ' ')
                {
                    StringBuilder move = new StringBuilder();
                    move.Append(i_CurrentPieceLocation);
                    move.Append('>');
                    move.Append(i_Board.GetCharLocationByIndexes(col + 2, row + 2));
                    listOfMoves.Add(move.ToString());
                }
            }
        }

        if (i_Board.IsValidIndex(col - 2, row + 2))
        {
            if (boardMatrix[row + 1, col - 1] == opponentPieceRegularShape ||
                boardMatrix[row + 1, col - 1] == opponentPieceKingShape)
            {
                if (boardMatrix[row + 2, col - 2] == ' ')
                {
                    StringBuilder move = new StringBuilder();
                    move.Append(i_CurrentPieceLocation);
                    move.Append('>');
                    move.Append(i_Board.GetCharLocationByIndexes(col - 2, row + 2));
                    listOfMoves.Add(move.ToString());
                }
            }
        }

        if (i_Board.IsValidIndex(col + 2, row - 2))
        {
            if (boardMatrix[row - 1, col + 1] == opponentPieceRegularShape ||
                boardMatrix[row - 1, col + 1] == opponentPieceKingShape)
            {
                if (boardMatrix[row - 2, col + 2] == ' ')
                {
                    StringBuilder move = new StringBuilder();
                    move.Append(i_CurrentPieceLocation);
                    move.Append('>');
                    move.Append(i_Board.GetCharLocationByIndexes(col + 2, row - 2));
                    listOfMoves.Add(move.ToString());
                }
            }
        }

        if (i_Board.IsValidIndex(col - 2, row - 2))
        {
            if (boardMatrix[row - 1, col - 1] == opponentPieceRegularShape ||
                boardMatrix[row - 1, col - 1] == opponentPieceKingShape)
            {
                if (boardMatrix[row - 2, col - 2] == ' ')
                {
                    StringBuilder move = new StringBuilder();
                    move.Append(i_CurrentPieceLocation);
                    move.Append('>');
                    move.Append(i_Board.GetCharLocationByIndexes(col - 2, row - 2));
                    listOfMoves.Add(move.ToString());
                }
            }
        }

        return listOfMoves;
    }

    public void ExtraEating(Board i_Board, string i_CurrentPieceLocation, ref bool io_IsSecondEating)
    {
        List<string> listOfMoves;

        if (i_Board.WhichPieceInSquare(i_CurrentPieceLocation) == 'X' || i_Board.WhichPieceInSquare(i_CurrentPieceLocation) == 'O')
        {
            listOfMoves = setListOfEatingValidMovesForRegularPieces(i_Board, i_CurrentPieceLocation);
        }
        else
        {
            listOfMoves = setListOfEatingValidMovesForKingPieces(i_Board, i_CurrentPieceLocation);
        }

        if (listOfMoves.Count > 0)
        {
            m_ListOfEatingValidMoves = listOfMoves;
            io_IsSecondEating = true;
        }
    }

    public void UpdatePlayer(Board i_Board)
    {
        setListOfRegularPieces(i_Board);
        setListOfKingPieces(i_Board);
        setListOfNonEatingValidMoves(i_Board);
        setListOfEatingValidMoves(i_Board);
    }

    public string ComputerMove()
    {
        string choosedMove;
        Random random = new Random();

        if (m_ListOfEatingValidMoves.Count > 0)
        {
            int randomIndex = random.Next(m_ListOfEatingValidMoves.Count);
            choosedMove = m_ListOfEatingValidMoves[randomIndex];
        }
        else
        {
            int randomIndex = random.Next(m_ListOfNonEatingValidMoves.Count);
            choosedMove = m_ListOfNonEatingValidMoves[randomIndex];
        }

        return choosedMove;
    }

    public bool TouchMoveRule(Board i_Board, string i_CurrentPieceLocation)
    {
        List<string> listOfEatingMoves = new List<string>();
        List<string> listOfNonEatingMoves = new List<string>();
        bool v_State = false;

        foreach (string move in m_ListOfEatingValidMoves)
        {
            if (move.Substring(0, 2).Equals(i_CurrentPieceLocation))
            {
                listOfEatingMoves.Add(move);
            }
        }

        foreach (string move in m_ListOfNonEatingValidMoves)
        {
            if (move.Substring(0, 2).Equals(i_CurrentPieceLocation))
            {
                listOfNonEatingMoves.Add(move);
            }
        }

        if ((listOfEatingMoves.Count + listOfNonEatingMoves.Count) > 0)
        {
            m_ListOfEatingValidMoves = listOfEatingMoves;
            m_ListOfNonEatingValidMoves = listOfNonEatingMoves;
            v_State = true;
        }

        return v_State;
    }
}