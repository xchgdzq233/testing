using FFX_P8_Image.Client.FFXImageServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFX_P8_Image.Client
{
    public class ImageProxy: ClientBase<IFFXImageService>, IFFXImageService
    {
        public ImageProxy() { }
        public ImageProxy(string endpointName) : base(endpointName) { }
        public ImageProxy(Binding binding, string address) : base(binding, new EndpointAddress(address)) { }

        public ObservableCollection<string> GetTicketsList(string sTicketNum)
        {
            return Channel.GetTicketsList(sTicketNum);
        }

        public Task<ObservableCollection<string>> GetTicketsListAsync(string sTicketNum)
        {
            return Channel.GetTicketsListAsync(sTicketNum);
        }
    }
}
