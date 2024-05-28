using business_logic_layer.ViewModel;

namespace business_logic_layer
{
    public interface ISendRegistration
    {
        Task SendRegistrationPendingEmail(UserRegistrationModel user);
        Task SendAccountActivatedEmail(UserRegistrationModel user);
    }
}

