﻿using MedCare.Application.Commands;
using MedCare.Application.PopUps;
using MedCare.Application.Services;
using MedCare.Application.ViewModels.Base;
using MedCare.Commons.Entities;
using MedCare.DB.Enums;
using MedCare.DB.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCare.Application.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IScreenControl _screenControl;
        public UIInformationPopUp InformationPopUp { get; set; }
        public RelayCommand RegisterCommand { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set
            {
                _cpf = value;
                OnPropertyChanged(nameof(CPF));
            }
        }

        private string _age;
        public string Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private string _cellPhone;
        public string CellPhone
        {
            get => _cellPhone;
            set
            {
                _cellPhone = value;
                OnPropertyChanged(nameof(CellPhone));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _job;
        public string Job
        {
            get => _job;
            set
            {
                _job = value;
                OnPropertyChanged(nameof(Job));
            }
        }

        private string _jobCode;
        public string JobCode
        {
            get => _jobCode;
            set
            {
                _jobCode = value;
                OnPropertyChanged(nameof(JobCode));
            }
        }

        private int _registerType;
        public int RegisterType
        {
            get => _registerType;
            set
            {
                _registerType = value;
                OnPropertyChanged(nameof(RegisterType));
            }
        }

        private bool _textBoxJobCodeIsVisible;
        public bool TextBoxJobCodeIsVisible
        {
            get => _textBoxJobCodeIsVisible;
            set
            {
                _textBoxJobCodeIsVisible = value;
                OnPropertyChanged(nameof(TextBoxJobCodeIsVisible));
            }
        }

        public RegistrationViewModel(IScreenControl screenControl)
        {
            _screenControl = screenControl;
            RegisterCommand = new RelayCommand(Register);
            InformationPopUp = new UIInformationPopUp(OpenLoginScreen) { ScreenName = "Registration" };
        }

        public void Register()
        {
            if (RegisterType == 0)
                RegisterProfessional();
            else
                Registerpatient();
        }

        private void Registerpatient()
        {
            ProfessionalRepository professionalRepository = new ProfessionalRepository(new ProfessionalDatabaseFactory(EnumDatabaseTypes.ForImplementation));
            Professional newProfessional = new Professional();
            newProfessional.Name = Name;
            newProfessional.Email = Email;
            newProfessional.Age = int.Parse(Age);
            newProfessional.Password = Password;
            newProfessional.PhoneNumber = CellPhone;
            newProfessional.ProfessionalRegister = JobCode;

            bool wasSuccessful = professionalRepository.AddNewProfessional(newProfessional).Result;

            if (wasSuccessful)
                InformationPopUp.showSuccessfulMessage();
            else
                InformationPopUp.showNotSuccessfulMessage();
        }

        private void RegisterProfessional()
        {
            PatientRepository patientRepository = new PatientRepository(new PatientDatabaseFactory(EnumDatabaseTypes.ForImplementation));
            Patient newPatient = new Patient();
            newPatient.Name = Name;
            newPatient.Email = Email;
            newPatient.Age = int.Parse(Age);
            newPatient.Password = Password;
            newPatient.PhoneNumber = CellPhone;
            bool wasSuccessful = patientRepository.AddNewPatient(newPatient).Result;

            if (wasSuccessful)
                InformationPopUp.showSuccessfulMessage();
            else
                InformationPopUp.showNotSuccessfulMessage();
        }

        public void OpenLoginScreen()
        {
            _screenControl.NavigateToLoginView();
        }
    }
}
