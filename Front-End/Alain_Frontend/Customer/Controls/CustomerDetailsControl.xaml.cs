using project_03.Global_Classes;
using project_03.Model;
using System.Windows.Controls;

namespace project_03.Customer.Controls
{
    /// <summary>
    /// Interaction logic for CustomerDetailsControl.xaml
    /// </summary>
    public partial class CustomerDetailsControl : UserControl
    {
        public CustomerDetailsControl()
        {
            InitializeComponent();
        }
        public void Load_Customer(Model.Customer customer)
        {
            if (customer == null) return;

            TxtFullName.Text = $"{customer.FirstName} {customer.LastName}";
            TxtNationalNumber.Text = $"CIN: {customer.NationalNo}";
            TxtNationalID.Text = customer.NationalNo;
            TxtEmail.Text = customer.Email;
            TxtPhone.Text = customer.Phone;
            TxtNationality.Text = customer.Nationality;
            TxtTotalRentals.Text = $"{customer.TotalRentals} Locations";
            TxtDateOfBirth.Text = Format.DateToLong(customer.DateOfBirth);
            TxtAddress.Text = (customer.Address == "") ? "N/A" : customer.Address;
        }
        public void ResetForm()
        {
            string s = "N/A";

            TxtFullName.Text = s;
            TxtNationalNumber.Text = s;
            TxtNationalID.Text = s;
            TxtEmail.Text = s;
            TxtPhone.Text = s;
            TxtNationality.Text = s;
            TxtTotalRentals.Text = s;
            TxtDateOfBirth.Text = s;
            TxtAddress.Text = s;
        }
    }
}