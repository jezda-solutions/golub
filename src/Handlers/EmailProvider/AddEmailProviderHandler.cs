using Golub.Interfaces.Repositories;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class AddEmailProviderCommand
    {
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public string Configuration { get; set; }

        public int FreePlanQty { get; set; }

        public int RemainingQty { get; set; }

        public int Period { get; set; }
    }

    public class AddEmailProviderHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<Guid>> Handle(AddEmailProviderCommand request)
        {
            var emailProvider = await _emailProviderRepository.AddAsync(new Entities.EmailProvider()
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Configuration = request.Configuration,
                FreePlanQty = request.FreePlanQty,
                Period = request.Period,
                RemainingQty = request.FreePlanQty
            });

            return Result<Guid>.Success(emailProvider.Id);
        }
    }
}
