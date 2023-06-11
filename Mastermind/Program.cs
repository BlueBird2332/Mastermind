
using System.Runtime.ExceptionServices;

namespace MyNamespace;

class Program
{
    const int NumberOfColours = 6;
    const int MaxNumberOfGuesses = 8;
    const int NumberOfSpots = 4;

    public class GuessResult
    {
        public int NumCorrect;
        public int NumCorrectValueWrongPlace;
    }

    public class PreviousGuess
    {
        public List<int> Guess = new List<int>();
        public GuessResult GuessResult { get; set; }
    }

    public static void PlayMastermind()
    {
        List<PreviousGuess> previousGuesses = new List<PreviousGuess>();

        for (int i = 0; i < MaxNumberOfGuesses; i++)
        {
            List<int> NextGuess = GetNextGuess(previousGuesses);

            if (NextGuess == null)
            {
                Console.WriteLine("YOU CHEATED");
                return;
            }
            Console.WriteLine(WriteToConsole(NextGuess));

            bool isUserResponseOfProperType = false;
            int NumberOfCorrectValues = 0;
            int NumberOfCorrectGuesses = 0;

            while (!isUserResponseOfProperType)
            {
                try
                {
                    Console.Write("Number of correct guesses: ");
                    NumberOfCorrectValues = int.Parse(Console.ReadLine());
                    Console.Write("Number of correct values but in wrong places: ");
                    NumberOfCorrectGuesses = int.Parse(Console.ReadLine());
                    isUserResponseOfProperType = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Podaj prawidłową wartość");
                }
            }      

            if (NumberOfCorrectValues == NumberOfSpots)
            {
                Console.WriteLine("You won");
                return;
            }


            GuessResult guessResult = new GuessResult()
            {
                NumCorrect = NumberOfCorrectValues,
                NumCorrectValueWrongPlace = NumberOfCorrectGuesses,
            };

            PreviousGuess previousGuess = new PreviousGuess()
            {
                Guess = NextGuess,
                GuessResult = guessResult,
            };
            previousGuesses.Add(previousGuess);

        }
        
    }

    private static List<int> GetNextMastermindBoard(List<int> previousBoard) 
    {
        List<int> boardCopy = new List<int>(previousBoard);

        for(int i = boardCopy.Count - 1; i >= 0; i--) 
        {
            if(boardCopy[i] < NumberOfColours - 1) 
            {
                boardCopy[i]++;
                break;
            }
            
            boardCopy[i] = 0;
        }

        return boardCopy;
    }

    public static List<int>? GetNextGuess(List<PreviousGuess> previousGuesses)
    {

        List<int> nextGuess = Enumerable.Repeat(0, NumberOfSpots).ToList();
        if(CheckMatchWithAllPrevious(previousGuesses, nextGuess)) 
        {
            return nextGuess;
        }

        List<int> lastPossibleGuess = Enumerable.Repeat(NumberOfColours - 1, NumberOfSpots).ToList();
        while(!nextGuess.SequenceEqual(lastPossibleGuess)) 
        {
            nextGuess = GetNextMastermindBoard(nextGuess);
        
            if(CheckMatchWithAllPrevious(previousGuesses, nextGuess)) 
            {
                return nextGuess;
            }
        }

        return null;    
    }

    public static bool CheckMatchWithAllPrevious(List<PreviousGuess> pastGuessList, List<int> valueToCheck)
    {
        for(int i = 0; i < pastGuessList.Count; i++)
        {
            GuessResult temporaryGuess = GetComparedGuessResult(valueToCheck, pastGuessList[i].Guess);

            if (temporaryGuess.NumCorrect != pastGuessList[i].GuessResult.NumCorrect || temporaryGuess.NumCorrectValueWrongPlace != pastGuessList[i].GuessResult.NumCorrectValueWrongPlace)
            {
                return false;
            }
        }
        return true;
    }

    public static GuessResult GetComparedGuessResult(List<int> toCheck, List<int> correctBoard)
    {
        int CorrectAmount = 0;
        int WrongPlaceAmount = 0;
        
        for(int i = 0; i < NumberOfColours;i++)
        {
            int OccurrenceInToCheck = 0;
            int OccurrenceInCorrectBoard = 0;

            for(int j=0; j < NumberOfSpots; j++)
            {
                if (correctBoard[j] == i)
                {
                    OccurrenceInCorrectBoard++;
                }
                if (toCheck[j] == i)
                {
                    OccurrenceInToCheck++; 
                }
                
            }
            WrongPlaceAmount += Math.Min(OccurrenceInToCheck, OccurrenceInCorrectBoard);
        }

        for(int i = 0; i < NumberOfSpots ;i++)
        {
            if (toCheck[i] == correctBoard[i])
            {
                CorrectAmount++;
                WrongPlaceAmount--;
            }
        }

        return new GuessResult
        {
            NumCorrect = CorrectAmount,
            NumCorrectValueWrongPlace = WrongPlaceAmount,
        };      
    }

    public static string WriteToConsole(List<int> Guess)
    {
        string ReturnString = "";
        for(int i = 0; i<NumberOfSpots; i++)
        {
            
            ReturnString += "[" + Guess[i] + "] ";
        ;
        }
        return ReturnString;
    }

    private static void Main(string[] args)
    {
        PlayMastermind();
    }
}