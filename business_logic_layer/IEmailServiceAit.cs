using System;
namespace business_logic_layer.ViewModel
{
    public interface IEmailServiceAit
    {
        Task SendEmailAsyncAit(mailRequestModelAit mailrequest);

    }

}

