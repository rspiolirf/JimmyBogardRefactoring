namespace JimmyRefactoring.Domain;

public class Member
{
    public int Id { get; set; }
    public String Email { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public int NumberOfActiveOffers { get; set; }
    public IList<Offer> AssignedOffers { get; set; } = new List<Offer>();

    public Member(int id, string email, string firstName, string lastName)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}