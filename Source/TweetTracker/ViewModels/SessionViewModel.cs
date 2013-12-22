﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetTracker.Model;

namespace TweetTracker.ViewModels
{
    class SessionViewModel : BaseViewModel
    {
        private ObservableCollection<CaptureSessionBase> _tabpages;

        private CaptureSession _session;

        public SessionViewModel()
        {
            this._tabpages = new ObservableCollection<CaptureSessionBase>();
        }

        public CaptureSession Session
        {
            get
            {
                return this._session;
            }

            set
            {
                this._session = value;
                this.OnPropertyChanged("Session");
            }
        }

        public ObservableCollection<CaptureSessionBase> Pages
        {
            get
            {
                return this._tabpages;
            }

            private set
            {
                this._tabpages = value;
                this.OnPropertyChanged("Pages");
            }
        }


        public void StartSession(CaptureSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            this.Pages.Clear();

            this.Session = session;
            this.Session.StartCaptureNonBlocking();

            var staticVm = new StaticSessionViewModel();
            staticVm.StartListening(session);

            this.Pages.Add(staticVm);
        }

        public void StopSession()
        {
            this.Session.StopCapture();

            foreach (var model in this.Pages)
            {
                model.StopListening();
            }

        }

        public void UpdateCaptureSettings(CaptureSettingsViewModel settingsModel)
        {
            var settings = new CaptureSettings(settingsModel);
            this.Session.UpdateCaptureSettings(settings);
        }
        
    }
}
