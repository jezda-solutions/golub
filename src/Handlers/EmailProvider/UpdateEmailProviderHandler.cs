using Golub.Interfaces.Repositories;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class UpdateEmailProviderCommand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public string Configuration { get; set; }

        public int FreePlanQty { get; set; }

        public int RemainingQty { get; set; }

        public int Period { get; set; }
    }

    public class UpdateEmailProviderHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<Guid>> Handle(UpdateEmailProviderCommand request)
        {
            var emailProvider = await _emailProviderRepository.GetFirstAsync(x => x.IsActive && x.Id == request.Id);

            if (emailProvider == null)
            {
                return Result<Guid>.Fail("Email provider not found");
            }

            emailProvider.Name = request.Name;
            emailProvider.IsActive = request.IsActive;
            emailProvider.Configuration = request.Configuration;
            emailProvider.FreePlanQty = request.FreePlanQty;
            emailProvider.Period = request.Period;
            emailProvider.RemainingQty = request.RemainingQty;

            await _emailProviderRepository.UpdateAsync(emailProvider);

            return Result<Guid>.Success(emailProvider.Id);
        }
    }
}
