using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.System;

namespace craftersmine.GameEngine.Network
{
    /// <summary>
    /// Represents game object with network properties
    /// </summary>
    public class NetworkGameObject : GameObject
    {
        /// <summary>
        /// Gets or sets that object can transmit it position on scene
        /// </summary>
        public bool IsTransmittingLocation { get; set; }
        /// <summary>
        /// Gets or sets that object can receive it position from server
        /// </summary>
        public bool IsReceivingLocation { get; set; }
        /// <summary>
        /// Gets or sets that object syncing animation from server
        /// </summary>
        public bool IsSyncingAnimation { get; set; }
        /// <summary>
        /// Gets or sets that object syncing texture from server
        /// </summary>
        public bool IsSyncingTexture { get; set; }
        /// <summary>
        /// Gets or sets that object can transmit it size on scene
        /// </summary>
        public bool IsTransmittingSize { get; set; }
        /// <summary>
        /// Gets or sets that object can receive it size from server
        /// </summary>
        public bool IsReceivingSize { get; set; }
        /// <summary>
        /// Gets current game object network ID
        /// </summary>
        public int NetworkId { get; internal set; }
        /// <summary>
        /// Gets or sets current game object network name
        /// </summary>
        public string NetworkObjectName { get; set; }

        /// <summary>
        /// Creates new instance of <see cref="NetworkGameObject"/>
        /// </summary>
        public NetworkGameObject()
        {

        }

        /// <summary>
        /// Calls at network sync
        /// </summary>
        public virtual void OnNetworkSync()
        {

        }
    }
}
