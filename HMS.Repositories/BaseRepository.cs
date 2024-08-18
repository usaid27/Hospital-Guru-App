using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HMS.Repositories
{
    public class BaseRepository
    {
        public BaseRepository(ILogger<BaseRepository> logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; private set; }
    }
}
