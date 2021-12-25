using OnVen.Common.Entities;
using OnVen.Common.Responses;
using OnVen.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace OnVen.Prism.ViewModels
{
	public class ProductsPageViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<Product> _product;
        private bool _isRunning;
        private string _search;
        private DelegateCommand _searchCommand;
        private List<Product> _myProducts;

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowProducts));

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowProducts();
            }
        }

        public ProductsPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Products";
            _navigationService = navigationService;
            _apiService = apiService;
            LoadProductAsync();
        }

        public ObservableCollection<Product> Products { 
            get => _product; 
            set => SetProperty(ref _product, value); 
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }


        private async void LoadProductAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection", "Accept");
                return;
            }

            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Product>(url, "/api", "/Products");
            IsRunning = false;
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            _myProducts = (List<Product>)response.Result;
            ShowProducts();


            return;
        }

        private void ShowProducts()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Products = new ObservableCollection<Product>(_myProducts);
            }
            else
            {
                Products = new ObservableCollection<Product>(_myProducts
                    .Where(p => p.Name.ToLower().Contains(Search.ToLower())));
            }
        }
    }
}
