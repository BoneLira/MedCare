﻿using MedCare.Application.Exceptions;
using MedCare.Commons.Entities;
using MedCare.DB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCare.Application.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Attributes and Constructor
        private IPatientRepository patientRepository;
        private IProfessionalRepository professionalRepository;

        public AuthenticationService()
        {
            //this.patientRepository = new PatientRepository();
            //this.professionalRepository = new ProfessionalRepository();
        }
        #endregion

        #region Validate Login
        public async Task<Tuple<EnumUserType, int>> ValidateLogin(string email, String password)
        {

            Patient patient = new Patient()
            {
                Email = email,
                Password = password
            };

            Patient patientResult = await patientRepository.GetPatient(patient);
            if (patientResult != null)
            {
                if (patientResult.Password == password)
                {
                    return new Tuple<EnumUserType, int>(EnumUserType.PATIENT, patientResult.Id);
                }
                else
                {
                    throw new IncorrectPasswordException("Incorrect password");
                }
            }

            Professional professional = new Professional()
            {
                Email = email,
                Password = password
            };

            Professional professionalResult = await professionalRepository.GetProfessional(professional);
            if (professionalResult != null)
            {
                if (professionalResult.Password == password)
                {
                    return new Tuple<EnumUserType, int>(EnumUserType.PROFESSIONAL, professionalResult.Id);
                }
                else
                {
                    throw new IncorrectPasswordException("Incorrect password");
                }
            }

            throw new NotFoundUserException("Not found user");
        }
        #endregion

        #region Validate Registration
        public Task ValidateRegistration(EnumUserType userType, string name, string cpf, int age, int contactNumber,
            string email, String password, String confirmPassword, string profession, string professionalNumber)
        {
            if (password != confirmPassword)
            {
                throw new PasswordsAreNotEqualsException("Passwords are not equals");
            }

            if (userType == EnumUserType.PATIENT)
            {
                return PatientRegistration(name, cpf, age, contactNumber, email, password);
            }
            else
            {
                return ProfessionalRegistration(name, cpf, age, contactNumber, email, password, profession, professionalNumber);
            }

        }

        private async Task PatientRegistration(string name, string cpf, int age, int contactNumber, string email, String password)
        {
            Patient patient = new Patient()
            {
                Name = name,
                Cpf = cpf,
                Age = age,
                PhoneNumber = contactNumber,
                Email = email,
                Password = password
            };

            Patient patientResult = await patientRepository.GetPatient(patient);
            if (patientResult != null)
            {
                throw new UserAlreadyExistsException("E-mail linked to an existing account");
            }

            await patientRepository.AddNewPatient(patient);
        }

        private async Task ProfessionalRegistration(string name, string cpf, int age, int contactNumber, string email,
            String password, string profession, string professionalNumber)
        {
            Professional professional = new Professional()
            {
                Email = email,
                Password = password
            };

            Professional professionalResult = await professionalRepository.GetProfessional(professional);
            if (professionalResult != null)
            {
                throw new UserAlreadyExistsException("E-mail linked to an existing account");
            }

            await professionalRepository.AddNewProfessional(professional);
        }
        #endregion

    }
}
