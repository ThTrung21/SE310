﻿using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewStaffViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewStaffViewModel));
        private Staff _currentstaff= new Staff();
        public Staff CurrentStaff
        {
            get => _currentstaff;
            set { _currentstaff = value; OnPropertyChanged(nameof(CurrentStaff)); }
        }
        private string _fullname;
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public ICommand UpdateStaff { get; set; }

        public ViewStaffViewModel() { _logger.Information("ViewStaffViewModel constructor called."); }
        public ViewStaffViewModel(Staff s) 
        {
            _logger.Information("ViewAuthorViewModel constructor with Customer {a} parameter called.", s.ID);
            this.CurrentStaff = s;
            UpdateStaff = new UpdateStaffCommand(this);
        }


    }
}
