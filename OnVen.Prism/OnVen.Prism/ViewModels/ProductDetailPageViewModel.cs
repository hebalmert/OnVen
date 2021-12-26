﻿using OnVen.Common.Entities;
using OnVen.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OnVen.Prism.ViewModels
{
    public class ProductDetailPageViewModel : ViewModelBase
    {
        private Product _product;
        private ObservableCollection<ProductImage> _images;

        public ProductDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Product;
        }

        public ObservableCollection<ProductImage> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<Product>("product");
                Title = Product.Name;
                Images = new ObservableCollection<ProductImage>(Product.ProductImages);
            }
        }

    }
}