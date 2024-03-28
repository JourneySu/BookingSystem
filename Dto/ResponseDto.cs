namespace BookingSystem.Dto;

public class ResponseDto
{
    public int statusCode { get; set; }
    public DateTime timestamp { get; set; }
    public string message { get; set; }
    public Result result { get; set; }
}
public class Situation
{
    public string date { get; set; }
    public string surplusText { get; set; }
    public bool isOffDay { get; set; }
    public bool isFull { get; set; }
}

public class Result
{
    public List<Situation> situation { get; set; }
}


