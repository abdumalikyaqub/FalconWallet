using FluentValidation;

namespace FalconWallet.API.Features.Transactions.SendToWallet;

public sealed class SendToWalletRequestValidator:AbstractValidator<SendToWalletRequest>
{
    public  SendToWalletRequestValidator()
    {
        RuleFor(r => r.FromWalletId)
            .NotEmpty().WithMessage("From WalletId is required");
        
        RuleFor(r => r.ToWalletId)
            .NotEmpty().WithMessage("To WalletId is required");
        
        RuleFor(r => r.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(r => r)
            .Must(r => r.FromWalletId != r.ToWalletId)
            .WithMessage("FromWalletId should not be equal or less than FromWalletId");
        
        RuleFor(r => r.Description)
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters");
    }
}