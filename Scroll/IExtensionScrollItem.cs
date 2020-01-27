using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ortega.Extensions
{
    public interface IExtensionScrollItem
    {
        void UpdateData<TExtensionBannerData>(TExtensionBannerData param)
            where TExtensionBannerData : ExtensionScrollItemData;
    }

    public interface IExtensionBanner : IExtensionScrollItem
    {
    }

    public interface IExtensionInfiniteScrollItem : IExtensionScrollItem
    {
    }
}
