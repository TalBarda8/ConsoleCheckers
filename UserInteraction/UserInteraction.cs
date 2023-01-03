using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

public class UserInteraction
{
    public static void Main(string[] args)
    {
        RunGame();
    }

    public static void RunGame()
    {
        Console.WriteLine("Hello player! Please enter your name:");
        string firstUserName = ReadUserName();
        int boardSize = ChooseBoardSize();
        string secondUserName = ReadOpponentName();

        Player firstPlayer = new Player(firstUserName, 'X');
        Player secondPlayer = new Player(secondUserName, 'O');

        StartGameRound(firstPlayer, secondPlayer, boardSize);

        Console.ReadLine();
    }

    public static string ReadUserName()
    {
        bool v_State = true;
        string userNameInput = string.Empty;
        int lettersCounter = 0;

        while (v_State)
        {
            userNameInput = Console.ReadLine();

            for (int i = 0; i < userNameInput.Length; i++)
            {
                if (Char.IsLetter(userNameInput[i]))
                {
                    lettersCounter++;
                }
            }

            if (userNameInput == "Computer")
            {
                Console.WriteLine("Please choose another name (which is not \"Computer\").");
                lettersCounter = 0;
            }
            else if (lettersCounter == userNameInput.Length)
            {
                Console.WriteLine($"Hello {userNameInput}!");
                v_State = false;
            }
            else
            {
                Console.WriteLine("Invalid name, please try again.");
                lettersCounter = 0;
            }
        }

        return userNameInput;
    }

    public static int ChooseBoardSize()
    {
        bool v_State = true;
        string boardSize = string.Empty;
        int intBoardSize = 0;

        Console.WriteLine("Please choose the size of the game board (6/8/10):");

        while (v_State)
        {
            boardSize = Console.ReadLine();

            if (!int.TryParse(boardSize, out intBoardSize))
            {
                Console.WriteLine("Invalid input, please try again.");
            }
            else
            {
                if (intBoardSize != 6 && intBoardSize != 8 && intBoardSize != 10)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
                else
                {
                    Console.WriteLine($"You chose a board size of {intBoardSize}x{intBoardSize}.");
                    v_State = false;
                }
            }
        }

        return intBoardSize;
    }

    public static string ReadOpponentName()
    {
        bool v_State = true;
        string opponentChoiceInput = string.Empty;
        string opponentName = string.Empty;

        Console.WriteLine("Would you like to play against the computer (write 'c') or against another player (write 'p')?");

        while (v_State)
        {
            opponentChoiceInput = Console.ReadLine();

            if (opponentChoiceInput == "c")
            {
                Console.WriteLine("You chose to play against the computer.");
                opponentName = "Computer";
                v_State = false;
            }
            else if (opponentChoiceInput == "p")
            {
                Console.WriteLine("You chose to play against another player.");
                Console.WriteLine("Please enter your Opponent's name:");
                opponentName = ReadUserName();
                v_State = false;
            }
            else
            {
                Console.WriteLine("Invalid name, please try again.");
            }
        }

        return opponentName;
    }

    public static void StartGameRound(Player io_FirstPlayer, Player io_SecondPlayer, int i_BoardSize)
    {
        bool v_GameStillGoingOn = true;
        Player winnerPlayer = null;
        Player lossingPlayer = null;
        Player currentTurnPlayer = io_FirstPlayer;
        Player notTurnPlayer = io_SecondPlayer;
        Board gameBoard = new Board(i_BoardSize);

        Screen.Clear();
        gameBoard.PrintBoard();
        Console.WriteLine($"{currentTurnPlayer.Name}'s Turn ({currentTurnPlayer.Shape}):");

        io_FirstPlayer.UpdatePlayer(gameBoard);
        io_SecondPlayer.UpdatePlayer(gameBoard);

        while (v_GameStillGoingOn)
        {
            string currentPlayerMove = Turn(currentTurnPlayer, notTurnPlayer, ref v_GameStillGoingOn, out winnerPlayer, gameBoard);

            if (currentPlayerMove == "Q")
            {
                break;
            }

            Screen.Clear();
            gameBoard.PrintBoard();
            CheckIfGameEnds(currentTurnPlayer, notTurnPlayer, ref v_GameStillGoingOn, out winnerPlayer);

            if (v_GameStillGoingOn)
            {
                if (currentTurnPlayer == io_FirstPlayer)
                {
                    currentTurnPlayer = io_SecondPlayer;
                    notTurnPlayer = io_FirstPlayer;
                }
                else if (currentTurnPlayer == io_SecondPlayer)
                {
                    currentTurnPlayer = io_FirstPlayer;
                    notTurnPlayer = io_SecondPlayer;
                }

                Console.WriteLine($"{notTurnPlayer.Name}'s move was ({notTurnPlayer.Shape}): {currentPlayerMove}");
                Console.WriteLine($"{currentTurnPlayer.Name}'s Turn ({currentTurnPlayer.Shape}):");
            }
        }

        if (winnerPlayer != null)
        {
            if (winnerPlayer == io_FirstPlayer)
            {
                lossingPlayer = io_SecondPlayer;
            }
            else
            {
                lossingPlayer = io_FirstPlayer;
            }

            int pointAddForWinner = Math.Abs(winnerPlayer.RegularPieces.Count + (4 * winnerPlayer.KingPieces.Count) - (lossingPlayer.RegularPieces.Count + (4 * lossingPlayer.KingPieces.Count)));
            Console.WriteLine($"{winnerPlayer.Name} wins this round! You get {pointAddForWinner} points.");
            winnerPlayer.Points += pointAddForWinner;
        }
        else
        {
            Console.WriteLine("Both Players have no valid moves. The round ends with a tie.");
        }

        bool v_GameRequestState = true;
        string anotherGameRequest;

        Console.WriteLine("Do you want to play again? (y/n)");

        while (v_GameRequestState)
        {
            anotherGameRequest = Console.ReadLine();

            if (anotherGameRequest == "y")
            {
                Console.WriteLine("Grate! New Game will start right up!");
                Console.WriteLine($"Current scroe:\n{io_FirstPlayer.Name} points: {io_FirstPlayer.Points}.\n{io_SecondPlayer.Name} points: {io_SecondPlayer.Points}.");
                Thread.Sleep(2000);
                Console.Write("Starting new game");

                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(1000);
                    Console.Write(".");
                }

                Thread.Sleep(1000);
                StartGameRound(io_FirstPlayer, io_SecondPlayer, i_BoardSize);
                v_GameRequestState = false;
            }
            else if (anotherGameRequest == "n")
            {
                Console.WriteLine($"Total score:\n{io_FirstPlayer.Name} points: {io_FirstPlayer.Points}.\n{io_SecondPlayer.Name} points: {io_SecondPlayer.Points}.");

                if (io_FirstPlayer.Points > io_SecondPlayer.Points)
                {
                    Console.WriteLine($"{io_FirstPlayer.Name} wins!");
                }
                else if (io_SecondPlayer.Points > io_FirstPlayer.Points)
                {
                    Console.WriteLine($"{io_SecondPlayer.Name} wins!");
                }
                else
                {
                    Console.WriteLine("It's a tie!");
                }

                Console.WriteLine("Thank you for playing! Bye bye!");
                v_GameRequestState = false;
            }
            else
            {
                Console.WriteLine("Invalid input, please try again.");
            }
        }
    }

    public static string Turn(Player io_CurrentTurnPlayer, Player io_NotTurnPlayer, ref bool o_v_GameStillGoingOn, out Player o_WinnerPlayer, Board io_GameBoard)
    {
        bool v_TurnState = true;
        string turnInput = string.Empty;
        o_WinnerPlayer = null;

        while (v_TurnState)
        {
            if (io_CurrentTurnPlayer.Name == "Computer")
            {
                turnInput = io_CurrentTurnPlayer.ComputerMove();
                Console.Write("The Computher is thinking about its next turn");

                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(1000);
                    Console.Write(".");
                }

                Thread.Sleep(1000);
            }
            else
            {
                turnInput = Console.ReadLine();
            }

            if (turnInput == "Q")
            {
                Console.WriteLine($"{io_CurrentTurnPlayer.Name} quits.");
                o_WinnerPlayer = io_NotTurnPlayer;
                v_TurnState = false;
                o_v_GameStillGoingOn = false;
            }
            else if (io_CurrentTurnPlayer.EatingValidMoves.Count == 0)
            {
                if (io_CurrentTurnPlayer.NonEatingValidMoves.Contains(turnInput))
                {
                    io_GameBoard.MakeMove(turnInput);
                    io_CurrentTurnPlayer.UpdatePlayer(io_GameBoard);
                    io_NotTurnPlayer.UpdatePlayer(io_GameBoard);
                    v_TurnState = false;
                }
                else
                {
                    Console.WriteLine("Invalid Move, please try again.");
                    if (io_CurrentTurnPlayer.TouchMoveRule(io_GameBoard, turnInput.Substring(0, 2)))
                    {
                        Console.WriteLine($"You touched piece \"{turnInput.Substring(0, 2)}\", now you must play with this piece :(");
                    }
                }
            }
            else
            {
                if (io_CurrentTurnPlayer.EatingValidMoves.Contains(turnInput))
                {
                    io_GameBoard.MakeMove(turnInput);
                    io_CurrentTurnPlayer.UpdatePlayer(io_GameBoard);
                    io_NotTurnPlayer.UpdatePlayer(io_GameBoard);
                    v_TurnState = false;
                    io_CurrentTurnPlayer.ExtraEating(io_GameBoard, turnInput.Substring(3,2), ref v_TurnState);

                    if (v_TurnState)
                    {
                        Screen.Clear();
                        io_GameBoard.PrintBoard();
                        Console.WriteLine($"{io_CurrentTurnPlayer.Name}'s move was ({io_CurrentTurnPlayer.Shape}): {turnInput}");
                        Console.WriteLine($"{io_CurrentTurnPlayer.Name}, type your extra eating move:");
                    }
                }
                else if (io_CurrentTurnPlayer.NonEatingValidMoves.Contains(turnInput))
                {
                    Console.WriteLine("There is a better move to make (eating move)");
                }
                else
                {
                    Console.WriteLine("Invalid Move, please try again.");
                    if (io_CurrentTurnPlayer.TouchMoveRule(io_GameBoard, turnInput.Substring(0, 2)))
                    {
                        Console.WriteLine($"You touched piece \"{turnInput.Substring(0, 2)}\", now you must play with this piece :(");
                    }
                }
            }
        }

        return turnInput;
    }

    public static void CheckIfGameEnds(Player i_CurrentTurnPlayer, Player i_NotTurnPlayer, ref bool o_v_GameStillGoingOn, out Player o_WinnerPlayer)
    {
        o_WinnerPlayer = null;

        if (i_NotTurnPlayer.RegularPieces.Count + i_NotTurnPlayer.KingPieces.Count == 0 || i_NotTurnPlayer.NonEatingValidMoves.Count + i_NotTurnPlayer.EatingValidMoves.Count == 0)
        {
            o_WinnerPlayer = i_CurrentTurnPlayer;
            o_v_GameStillGoingOn = false;
            return;
        }

        if (i_CurrentTurnPlayer.NonEatingValidMoves.Count + i_CurrentTurnPlayer.EatingValidMoves.Count == 0 && i_NotTurnPlayer.NonEatingValidMoves.Count + i_NotTurnPlayer.EatingValidMoves.Count == 0)
        {
            o_v_GameStillGoingOn = false;
        }
    }
}
