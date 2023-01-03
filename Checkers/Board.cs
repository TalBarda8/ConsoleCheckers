using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Board
{
    private readonly int m_RowAndColLength;
    private char[,] m_BoardMatrix;

    public Board(int i_RowAndColLength)
    {
        m_RowAndColLength = i_RowAndColLength;
        m_BoardMatrix = new char[m_RowAndColLength, m_RowAndColLength];
        this.putPiecesOnBoard();
    }

    public char[,] GetBoardMatrix()
    {
        return m_BoardMatrix;
    }

    public int Length
    {
        get
        {
            return m_RowAndColLength;
        }
    }

    private void putPiecesOnBoard()
    {
        for (int i = 0; i < m_RowAndColLength; i++)
        {
            for (int j = 0; j < m_RowAndColLength; j++)
            {
                m_BoardMatrix[i, j] = ' ';
            }
        }

        for (int i = 0; i < (m_RowAndColLength / 2) - 1; i++)
        {
            for (int j = 0; j < m_RowAndColLength; j++)
            {
                if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                {
                    m_BoardMatrix[i, j] = 'O';
                }
            }
        }

        for (int i = m_RowAndColLength - 1; i > (m_RowAndColLength / 2); i--)
        {
            for (int j = 0; j < m_RowAndColLength; j++)
            {
                if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                {
                    m_BoardMatrix[i, j] = 'X';
                }
            }
        }
    }

    private void deletePiecesInDiagonal(int i_ColIndex1, int i_RowIndex1, int i_ColIndex2, int i_RowIndex2)
    {
        int rowDirection;
        int colDirection;

        if (i_ColIndex2 - i_ColIndex1 > 0)
        {
            colDirection = 1;
        }
        else
        {
            colDirection = -1;
        }

        if (i_RowIndex2 - i_RowIndex1 > 0)
        {
            rowDirection = 1;
        }
        else
        {
            rowDirection = -1;
        }

        i_ColIndex1 += colDirection;
        i_RowIndex1 += rowDirection;

        while (i_ColIndex1 != i_ColIndex2 || i_RowIndex1 != i_RowIndex2)
        {
            m_BoardMatrix[i_RowIndex1, i_ColIndex1] = ' ';
            i_ColIndex1 += colDirection;
            i_RowIndex1 += rowDirection;
        }
    }

    public void PrintBoard()
    {
        char[] capitalLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        char[] lowerLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };

        for (int i = 0; i < m_RowAndColLength; i++)
        {
            Console.Write("   ");
            Console.Write(capitalLetters[i]);
        }

        Console.WriteLine("  ");
        StringBuilder sepLine = new StringBuilder();
        sepLine.Append(' ', 1);
        sepLine.Append('=', (3 * m_RowAndColLength) + m_RowAndColLength + 1);
        Console.WriteLine(sepLine.ToString());

        for (int i = 0; i < m_RowAndColLength; i++)
        {
            Console.Write(lowerLetters[i] + "|");

            for (int j = 0; j < m_RowAndColLength; j++)
            {
                Console.Write(" {0} |", m_BoardMatrix[i, j]);
            }

            Console.WriteLine();
            Console.WriteLine(sepLine.ToString());
        }
    }

    public void MakeMove(string i_Move)
    {
        int currentCol = GetIndexByChar(char.Parse(i_Move.Substring(0, 1)));
        int currentRow = GetIndexByChar(char.Parse(i_Move.Substring(1, 1)));
        int newCol = GetIndexByChar(char.Parse(i_Move.Substring(3, 1)));
        int newRow = GetIndexByChar(char.Parse(i_Move.Substring(4, 1)));
        char pieceShape = m_BoardMatrix[currentRow, currentCol];
        m_BoardMatrix[newRow, newCol] = pieceShape;
        m_BoardMatrix[currentRow, currentCol] = ' ';

        this.deletePiecesInDiagonal(currentCol, currentRow, newCol, newRow);

        if (pieceShape == 'X' && newRow == 0)
        {
            m_BoardMatrix[newRow, newCol] = 'Z';
        }
        else if (pieceShape == 'O' && newRow == m_RowAndColLength -1)
        {
            m_BoardMatrix[newRow, newCol] = 'Q';
        }
    }

    public char WhichPieceInSquare(string i_Locate)
    {
        int col = GetIndexByChar(char.Parse(i_Locate.Substring(0, 1)));
        int row = GetIndexByChar(char.Parse(i_Locate.Substring(1, 1)));

        return m_BoardMatrix[row, col];
    }

    public int GetIndexByChar(char i_C)
    {
        int index = -1;

        if (i_C >= 'A' && i_C <= m_RowAndColLength + 64)
        {
            index = i_C - 65;
        }
        else if (i_C >= 'a' && i_C <= m_RowAndColLength + 96)
        {
            index = i_C - 97;
        }

        return index;
    }

    public string GetCharLocationByIndexes(int i_Col, int i_Row)
    {
        StringBuilder location = new StringBuilder();
        char c1 = (char)(i_Col + 65);
        char c2 = (char)(i_Row + 97);
        location.Append(c1);
        location.Append(c2);

        return location.ToString();
    }

    public bool IsValidIndex(int i_Col, int i_Row)
    {
        if (i_Col >= m_RowAndColLength || i_Col < 0 || i_Row >= m_RowAndColLength || i_Row < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}