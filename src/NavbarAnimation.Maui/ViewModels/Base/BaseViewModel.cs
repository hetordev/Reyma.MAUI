using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using NavbarAnimation.Maui.DataStores;
using NavbarAnimation.Maui.Models.Respones.Base;
using Sharpnado.CollectionView.ViewModels;
using Sharpnado.TaskLoaderView;

namespace NavbarAnimation.Maui.ViewModels.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract void Load();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TDataStore"></typeparam>
    public abstract partial class BaseListViewModel<TEntity, TResponse, TDataStore> : BaseViewModel 
        where TEntity : class
        where TResponse : BaseResponse 
        where TDataStore : ReymaBaseDataStore<TEntity, TResponse>
    {
        protected readonly TDataStore DataStore;

        public BaseListViewModel(TDataStore dataStore)
        {
            currentIndex = 0;

            DataStore = dataStore;

            EntityLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<TResponse>>();
            EntityLoaderNotifier.Load(async _ =>
            {
                await Task.CompletedTask;
                return Enumerable.Empty<TResponse>().ToList().AsReadOnly();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual TaskLoaderNotifier<IReadOnlyCollection<TResponse>> EntityLoaderNotifier { get; }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            DataSource = new ObservableRangeCollection<TResponse>();

            EntityLoaderNotifier.Load(async _ =>
            {
                var pagedResponse = await DataStore.GetList(pageSize: 1);
                var results = pagedResponse.Results.ToList();

                DataSource.AddRange(results);

                return results.AsReadOnly();
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private ObservableRangeCollection<TResponse> dataSource;

        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private int currentIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task InitialLoad()
        {
            await Task.CompletedTask;
        }
    }
}
