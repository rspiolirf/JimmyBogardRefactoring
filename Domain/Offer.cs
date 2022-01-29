namespace JimmyRefactoring.Domain;

public class Offer
{
    public int Id { get; set; }
    public Member? MemberAssigned { get; set; }
    public OfferType? Type { get; set; }
    public int Value { get; set; }
    public DateTime DateExpiring { get; set; }

    private Offer()
    {
    }
    
    public Offer(Member memberAssigned, OfferType type, int value, DateTime dateExpiring)
    {
        MemberAssigned = memberAssigned;
        Type = type;
        Value = value;
        DateExpiring = dateExpiring;
    }
}