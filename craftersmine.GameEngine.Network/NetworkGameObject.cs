using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.System;

namespace craftersmine.GameEngine.Network
{
    public class NetworkGameObject : GameObject
    {
        public bool IsTransmittingLocation { get; set; }
        public bool IsReceivingLocation { get; set; }
        public bool IsSyncingAnimation { get; set; }
        public bool IsSyncingTexture { get; set; }
        public bool IsTransmittingSize { get; set; }
        public bool IsReceivingSize { get; set; }

        public NetworkGameObject()
        {

        }

        public virtual void OnNetworkSync()
        {

        }
    }
}
