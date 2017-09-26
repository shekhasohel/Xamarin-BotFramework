using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList
{
    public interface IBotConnection
    {
        Task SendMessageAsync(string message);

        Task<string> GetMessagesAsync();
    }
}
