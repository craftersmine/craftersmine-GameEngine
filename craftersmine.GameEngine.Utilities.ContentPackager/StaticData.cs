using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Utilities.ContentPackager
{
    public class StaticData
    {
        public static Dictionary<string, ContentAsset> ContentAssets { get; set; } = new Dictionary<string, ContentAsset>();
        public static ContentAsset WizardContentAsset;

        public static SelectFileAndNameForm SelectFileAndNameForm { get; set; }
        public static AddContentForm AddContentForm { get; set; }

        public delegate void ContentAssetAddedEventDelegate(object sender, ContentAssetAddedEventArgs e);
        public static event ContentAssetAddedEventDelegate ContentAssetAddedEvent;

        public static bool IsContentAddCanceled { get; set; }

        public static void CallEvent()
        {
            ContentAssetAddedEvent?.Invoke(null, new ContentAssetAddedEventArgs() { ContentAsset = WizardContentAsset });
        }
    }

    public class ContentAssetAddedEventArgs
    {
        public ContentAsset ContentAsset { get; set; }
    }

    public struct ContentAsset
    {
        public ContentType ContentType { get; set; }
        public string AssetName { get; set; }
        public string AssetPath { get; set; }
    }
}
