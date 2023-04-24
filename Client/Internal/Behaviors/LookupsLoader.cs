using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.Behaviors
{
    public class LookupsLoader
    {
        private readonly LookupsVM _lookupsVM;

        public LookupsLoader(LookupsVM lookupsVM)
        {
            _lookupsVM = lookupsVM;
            _lookupsVM.PropertyChanged += LookupsVM_PropertyChanged;
        }

        private void LookupsVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LookupsVM.SelectedLookupCode):
                    LoadSelectedLookup(_lookupsVM.SelectedLookupCode);
                    break;
            }
        }

        private void LoadSelectedLookup(string code)
        {
            _lookupsVM.SelectedLookup = null;
            if (!string.IsNullOrEmpty(code))
            {
                Task.Run(() =>
                {
                    using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                    {
                        ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                        ILookupService lookupService = scope.Resolve<ILookupService>();
                        try
                        {
                            return lookupService.Get(settingsFactory.CreateApi(), code).Result;
                        }
                        catch (BrassLoon.RestClient.Exceptions.RequestError ex)
                        {
                            if (ex.StatusCode == HttpStatusCode.NotFound)
                                return null;
                            else
                                throw;
                        }
                        catch (AggregateException ex)
                        {
                            if (ex.InnerException != null && typeof(BrassLoon.RestClient.Exceptions.RequestError).Equals(ex.InnerException.GetType()))
                                if (((BrassLoon.RestClient.Exceptions.RequestError)ex.InnerException).StatusCode == HttpStatusCode.NotFound)
                                    return null;
                                else
                                    throw;
                            else
                                throw;
                        }
                    }
                })
                    .ContinueWith(LoadSelectedLookupCallback, code, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task LoadSelectedLookupCallback(Task<Lookup> load, object state)
        {
            Lookup lookup = null;
            try
            {
                lookup = await load;
                if (lookup == null)
                {
                    lookup = new Lookup
                    {
                        Code = (string)state,
                        Data = new Dictionary<string, string>()
                    };
                }
                _lookupsVM.SelectedLookup = LookupVM.Create(lookup, _lookupsVM);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }

        public void Load()
        {
            _lookupsVM.BusyVisibility = Visibility.Visible;
            _lookupsVM.LookupCodes.Clear();
            Task.Run(() =>
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    ILookupService lookupService = scope.Resolve<ILookupService>();
                    return lookupService.GetIndex(settingsFactory.CreateApi()).Result;
                }
            })
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<string>> load, object state)
        {
            try
            {
                _lookupsVM.LookupCodes.Clear();
                foreach (string code in await load)
                {
                    _lookupsVM.LookupCodes.Add(code);
                }
                if (_lookupsVM.LookupCodes.Count > 0) 
                    _lookupsVM.SelectedLookupCode = _lookupsVM.LookupCodes[0];
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _lookupsVM.BusyVisibility = Visibility.Collapsed;
            }
        }
    }
}
