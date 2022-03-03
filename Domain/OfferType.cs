namespace JimmyRefactoring.Domain;

public class OfferType
{
    public int Id { get; set; }
    public DateTime BeginDate { get; set; }
    public int DaysValid { get; set; }
    public ExpirationType ExpirationType { get; set; }
    public String Name { get; set; }
    public Offer? Offer { get; set; }
    public int OfferId { get; set; }

    public OfferType(int id, DateTime beginDate, int daysValid, ExpirationType expirationType, string name)
    {
        Id = id;
        BeginDate = beginDate;
        DaysValid = daysValid;
        ExpirationType = expirationType;
        Name = name;
    }
}