using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amizade.Infrastructure.Services.Queue
{
    public interface IQueueService
    {
        Task SendAsync(string messageText);
    }
}
