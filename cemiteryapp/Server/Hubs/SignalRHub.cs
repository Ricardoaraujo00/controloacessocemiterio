using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cemiteryapp.Server.Hubs
{
    public class SignalRHub : Hub
    {
        public static int numActual;
        public static int numMaximo;
        public async Task AdicionarPessoa()
        {
            numActual++;
            await Clients.All.SendAsync("actualNumber", numActual);
        }

        public async Task RemoverPessoa()
        {
            numActual--;
            await Clients.All.SendAsync("actualNumber", numActual);
        }

        
    }
}
