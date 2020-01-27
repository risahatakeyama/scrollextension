using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ortega.Core;
using UnityEngine;
using SuperScrollView;

namespace Ortega.Extensions
{
    public class ScrollExtensions
    {
        //拡張クラスではないかも…。毎度インスタンス生成して使う

        private List<ExtensionScrollItemData> ScrollItemDataList { get; set; }
        private Action<int> DotAction { get; set; }

        /// <summary>
        /// ループするバナーの初期化(ボタン付き
        /// </summary>
        /// <typeparam name="TExtensionBanner"></typeparam>
        /// <typeparam name="TExtensionBannerData"></typeparam>
        /// <param name="view"></param>
        /// <param name="dotAction"></param>
        /// <param name="bannerDataList"></param>
        public void InitBannerLoop<TExtensionBanner, TExtensionBannerData>(LoopListView2 view, Action<int> dotAction, List<TExtensionBannerData> bannerDataList)
            where TExtensionBanner : IExtensionBanner
            where TExtensionBannerData : ExtensionScrollItemData
        {
            var initParam = new LoopListViewInitParam
            {
                mSmoothDumpRate = 0.1f,
                mSnapVecThreshold = 99999,
            };

            ScrollItemDataList = new List<ExtensionScrollItemData>(bannerDataList);
            DotAction = dotAction;

            view.mOnSnapNearestChanged = OnUpdateBannerDot;
            view.InitListView(-1, OnUpdateBanner<TExtensionBanner>, initParam);

            //自動スクロール(Intervalで図る
        }

        /// <summary>
        /// 無限スクロールの初期化
        /// TODO 未完成 あとでいい感じに実装する
        /// </summary>
        /// <typeparam name="TExtensionInfiniteScroll"></typeparam>
        /// <typeparam name="TExtensionInfiniteScrollData"></typeparam>
        /// <param name="view"></param>
        /// <param name="scrollDataList"></param>
        public void InitInfiniteScroll<TExtensionInfiniteScroll, TExtensionInfiniteScrollData>(LoopListView2 view, List<TExtensionInfiniteScrollData> scrollDataList)
            where TExtensionInfiniteScroll : IExtensionInfiniteScrollItem
            where TExtensionInfiniteScrollData : ExtensionScrollItemData
        {
            OrtegaLogger.Log("無限スクロールの初期化");
            ScrollItemDataList = new List<ExtensionScrollItemData>(scrollDataList);
            view.InitListView(ScrollItemDataList.Count, OnUpdateInfiniteScroll<TExtensionInfiniteScroll>);
        }

        /// <summary>
        /// バナードットを更新する
        /// </summary>
        /// <param name="view"></param>
        /// <param name="item"></param>
        private void OnUpdateBannerDot(LoopListView2 view, LoopListViewItem2 item)
        {
            int itemIndex = view.CurSnapNearestItemIndex;
            int count = ScrollItemDataList.Count;
            int currentIndex = itemIndex % count;

            //ドットをオンオフする処理
            //currentIndexとリストのIndexの比較の処理が入る
            DotAction(currentIndex);
        }

        /// <summary>
        /// バナーを更新する
        /// </summary>
        /// <param name="view"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private LoopListViewItem2 OnUpdateBanner<TExtensionBanner>(LoopListView2 view, int index) where TExtensionBanner : IExtensionBanner
        {
            var itemObj = view.NewListViewItem(view.ItemPrefabDataList.First().mItemPrefab.name);

            int count = ScrollItemDataList.Count;

            int wrapindex = 0 <= index
                ? index % count
                : count + ((index + 1) % count) - 1;

            var itemUI = itemObj.GetComponent<TExtensionBanner>();

            var data = ScrollItemDataList[wrapindex];
            itemUI.UpdateData(data);

            return itemObj;
        }

        /// <summary>
        /// 無限スクロールを更新する
        /// TODO 未完成　あとでいい感じに実装する
        /// </summary>
        /// <typeparam name="TExtensionInfiniteScroll"></typeparam>
        /// <param name="view"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private LoopListViewItem2 OnUpdateInfiniteScroll<TExtensionInfiniteScroll>(LoopListView2 view, int index)
            where TExtensionInfiniteScroll : IExtensionInfiniteScrollItem
        {
            var itemObj = view.NewListViewItem(view.ItemPrefabDataList.First().mItemPrefab.name);
            return itemObj;
        }

        private static void SetSnapIndex(int offset, LoopListView2 view)
        {
            //m_timer = 0;
            int currentIndex = view.CurSnapNearestItemIndex;
            int nextIndex = currentIndex + offset;
            view.SetSnapTargetItemIndex(nextIndex);
        }

        //項目の追加はSetListItemCount
    }

    public class ExtensionScrollItemData
    {
    }
}
