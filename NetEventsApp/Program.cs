using System.Threading.Channels;

Game game = new Game(10);
//game.Notify += (mess) => Console.WriteLine(mess);
game.Notify += GreenPrint;


game.GoodGame(100);
game.LoosGame(20);

game.Notify -= GreenPrint;

game.GoodGame(100);
game.LoosGame(20);

void GreenPrint(object sender, GameEventArgs e)
{
    ConsoleColor color = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"{e.Message}, total score: {e.ScoreTotal}");
    Console.ForegroundColor = color;
}

class GameEventArgs
{
    public string Message { set; get; }
    public int ScoreTotal { set; get; }
    public int ScoreChange { set; get; }
    public GameEventArgs(string message, int scoreTotal, int scoreChange)
    {
        Message = message;
        ScoreTotal = scoreTotal;
        ScoreChange = scoreChange;
    }
}

class Game
{
    public delegate void ScoreHandler(object sender, GameEventArgs e);

    event ScoreHandler notify;
    public event ScoreHandler Notify
    {
        add
        {
            Console.WriteLine($"Method {value.Method.Name} add");
            notify += value;
        }
        remove
        {
            Console.WriteLine($"Method {value.Method.Name} remove");
            notify -= value;
        }
    }
    
    int score;
    public int Score 
    {
        set
        {
            if(score < 0)
                score = 0;
            score = value;
        }
        get => score;
    }
    public Game(int score) => Score = score;
    public void GoodGame(int score)
    {
        notify?.Invoke(this, new($"Game score add {score}", Score, score));
        Score += score;
    }
    public void LoosGame(int score)
    {
        if (Score >= score)
        {
            notify?.Invoke(this, new($"Game score delete {score}", Score, score));
            Score -= score;
        }
    }
}

