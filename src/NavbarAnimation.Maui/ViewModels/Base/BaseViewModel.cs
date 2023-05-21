using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using NavbarAnimation.Maui.DataStores;
using NavbarAnimation.Maui.Models.Respones.Base;
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
            DataStore = dataStore;

            EntityLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<TResponse>>();
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
            EntityLoaderNotifier.Load(async _ =>
            {
                await Task.Delay(2500);

                var pagedResponse = await DataStore.GetList(pageSize: 7);

                return pagedResponse.Results.ToList().AsReadOnly();
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private IEnumerable<TResponse> dataSource;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task InitialLoad()
        {
            var pagedResponse = await DataStore.GetList(pageSize: 7);

            DataSource = pagedResponse.Results;
        }
    }
}
