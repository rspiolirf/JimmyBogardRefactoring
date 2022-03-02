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
            // get database info
            var member = await _appDbContext.Members.FindAsync(request.MemberId) ?? throw new Exception();
            var offerType  = await _appDbContext.OfferTypes.FindAsync(request.OfferTypeId) ?? throw new Exception();

            // calculate offer value
            var value = await "https://localhost:7107"
                .AppendPathSegment("/calculate-offer-value")
                .SetQueryParams(new {
                    email = member.Email,
                    offerType = offerType.Name
                })
                .WithClient(new FlurlClient(new HttpClient(new HttpClientHandler {ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true})))
                .GetJsonAsync<int>();

            // calculate date expiring
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

            // assign offer
            var offer = new Offer(member, offerType, value, dateExpiring);

            member.AssignedOffers.Add(offer);
            member.NumberOfActiveOffers++;

            // save to database
            await _appDbContext.Offers.AddAsync(offer, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}