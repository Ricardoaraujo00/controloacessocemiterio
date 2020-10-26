using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cemiteryapp.Server.Hubs;

namespace cemiteryapp.Server 
{

    
    [ApiController]
    public class ApiService : ControllerBase
    {

        [HttpGet]
        [Route("api/initialData")]
        public Dictionary<string,int> InitialData()
        {
            Dictionary<string, int> initialData = new Dictionary<string, int>();

            initialData["actual"] = SignalRHub.numActual;
            initialData["max"] = SignalRHub.numMaximo;

            return initialData;
        }
    }
}
