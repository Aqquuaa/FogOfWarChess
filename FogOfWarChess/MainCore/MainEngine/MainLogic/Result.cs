namespace FogOfWarChess.MainCore.MainEngine;

public class Result
{
    public Color Winner { get; }
    public EndReason Reason { get; }

    public Result(Color winner, EndReason reason)
    {
        Winner = winner;
        Reason = reason;
    }

    public static Result Win(Color winner)
    {
        return new Result(winner, EndReason.Checkmate);
    }

    public static Result Draw(EndReason reason)
    {
        return new Result(Color.None, reason);
    }
}