﻿using CatanClient.Commands;
using CatanClient.Gameplay.Helpers;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.Gameplay.ViewModels
{
    internal class TradeWindowViewModel : ViewModelBase
    {
        private readonly AccountService.ProfileDto profile;
        private readonly ServiceManager serviceManager;
        private readonly GameService.GameDto game;
        public ObservableCollection<Resource> ResourcesToOffer { get; set; }
        public ObservableCollection<Resource> ResourcesToRequest { get; set; }

        public ICommand SendTradeCommand { get; }

        public TradeWindowViewModel(GameDto game, ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            profile = serviceManager.ProfileSingleton.Profile;
            this.game = game;

            ResourcesToOffer = new ObservableCollection<Resource>
            {
                new Resource { Name = Utilities.LUNAR_STONE, ImageSource = Utilities.LUNAR_STONE_IMAGE_PATH},
                new Resource { Name = Utilities.TRITONIUM, ImageSource = Utilities.TRITONIUM_IMAGE_PATH},
                new Resource { Name = Utilities.ALPHA_NANOFIBERS, ImageSource = Utilities.ALPHA_NANOFIBERS_IMAGE_PATH},
                new Resource { Name = Utilities.EPSILON_BIOMASS, ImageSource = Utilities.EPSILON_BIOMASS_BIOME_IMAGE_PATH},
                new Resource { Name = Utilities.GRX_810, ImageSource = Utilities.GRX_810_IMAGE_PATH}
            };

            ResourcesToRequest = new ObservableCollection<Resource>(ResourcesToOffer.Select(r => new Resource { Name = r.Name, ImageSource = r.ImageSource }));

            SendTradeCommand = new RelayCommand(OnSendTradeAsync);
        }

        private PlayerResourcesDto InicializateResources()
        {
            PlayerResourcesDto resources = new PlayerResourcesDto
            {
                LunarStone = new ResourceDto(),
                Grx810 = new ResourceDto(),
                AlphaNanofibers = new ResourceDto(),
                Tritonium = new ResourceDto(),
                EpsilonBiomass = new ResourceDto()
            };

            return resources;
        }

        private PlayerResourcesDto LoadNeedResources()
        {
            PlayerResourcesDto needResources = InicializateResources();
            needResources.PlayerId = profile.Id.Value;

            foreach (var resource in ResourcesToRequest)
            {
                switch (resource.Name)
                {
                    case "Lunar Stone":
                        needResources.LunarStone.Quantity = resource.Quantity;
                        break;
                    case "Grx810":
                        needResources.Grx810.Quantity = resource.Quantity;
                        break;
                    case "Alpha Nanofibers":
                        needResources.AlphaNanofibers.Quantity = resource.Quantity;
                        break;
                    case "Tritonium":
                        needResources.Tritonium.Quantity = resource.Quantity;
                        break;
                    case "Epsilon Biomass":
                        needResources.EpsilonBiomass.Quantity = resource.Quantity;
                        break;
                }
            }
            return needResources;
        }

        private PlayerResourcesDto LoadOfferResources()
        {
            PlayerResourcesDto offerResources = InicializateResources(); 
            offerResources.PlayerId = profile.Id.Value;
            foreach (var resource in ResourcesToOffer)
            {
                switch (resource.Name)
                {
                    case "Lunar Stone":
                        offerResources.LunarStone.Quantity = resource.Quantity;
                        break;
                    case "Grx810":
                        offerResources.Grx810.Quantity = resource.Quantity;
                        break;
                    case "Alpha Nanofibers":
                        offerResources.AlphaNanofibers.Quantity = resource.Quantity;
                        break;
                    case "Tritonium":
                        offerResources.Tritonium.Quantity = resource.Quantity;
                        break;
                    case "Epsilon Biomass":
                        offerResources.EpsilonBiomass.Quantity = resource.Quantity;
                        break;
                }
            }

            return offerResources;
        }

        private void OnSendTradeAsync()
        {
            PlayerResourcesDto needResources = LoadNeedResources();
            PlayerResourcesDto offerResources = LoadOfferResources();

            OperationResultDto result;

            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_LOADING_SCREEN, null);
                result = await serviceManager.GameServiceClient.SendTradeRequestAsync(needResources, offerResources, game);

                if (result.IsSuccess)
                {
                    MessageBox.Show("Se ha enviado el tradeo exitosamente");
                }
                else
                {
                    MessageBox.Show("No se ha podido enviar el tradeo");
                }
            });
            
        }


    }
}
