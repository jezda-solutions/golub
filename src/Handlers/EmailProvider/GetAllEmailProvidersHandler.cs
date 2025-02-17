using Golub.Interfaces.Repositories;
using Golub.Responses.HandlerResponse.EmailProviderResponse;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class GetAllEmailProvidersHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<IEnumerable<GetAllEmailProvidersResponse>>> Handle()
        {
            var emailProvider = await _emailProviderRepository.GetAsync();

            var data = emailProvider.Select(x => new GetAllEmailProvidersResponse
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                HasConfiguration = !string.IsNullOrEmpty(x.Configuration),
                FreePlanQty = x.FreePlanQty,
                RemainingQty = x.RemainingQty,
                Period = x.Period
            });

            return Result<IEnumerable<GetAllEmailProvidersResponse>>.Success(data);

        }
    }
}
