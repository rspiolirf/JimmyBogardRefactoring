using Flurl;
using Flurl.Http;
using JimmyRefactoring.Domain;
using JimmyRefactoring.Infrastructure;
using MediatR;

namespace JimmyRefactoring.Features.Offers.AssignOffer;

public class AssignOfferRequest : IRequest
{
    public int MemberId { get; set; }
    public int OfferTypeId { get; set; }

    public class AssignOfferRequestHandler : IRequestHandler<AssignOfferRequest>
    {
        private readonly AppDbContext _appDbContext;

        public AssignOfferRequestHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(AssignOfferRequest request, CancellationToken cancellationToken)
        {
            var member = await _appDbContext.Members.FindAsync(request.MemberId) ?? throw new Exception();
            var offerType  = await _appDbContext.OfferTypes.FindAsync(request.OfferTypeId) ?? throw new Exception();

            var value = await "https://localhost:7107".AppendPathSegment($"/calculate-offer-value?email={member.Email}&offerType={offerType.Name}")
                .WithClient(new FlurlClient(new HttpClient(new HttpClientHandler {ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true})))
                .GetJsonAsync<int>();

            DateTime dateExpiring;
            switch (offerType.ExpirationType)
            {
                case ExpirationType.Assignment:
                    dateExpiring = DateTime.Today.AddDays(offerType.DaysValid);
                    break;
                
                case ExpirationType.Fixed:
                    dateExpiring = offerType.BeginDate.AddDays(offerType.DaysValid);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var offer = new JimmyRefactoring.Domain.Offer(member, offerType, value, dateExpiring);

            member.AssignedOffers.Add(offer);
            member.NumberOfActiveOffers++;

            await _appDbContext.Offers.AddAsync(offer, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}