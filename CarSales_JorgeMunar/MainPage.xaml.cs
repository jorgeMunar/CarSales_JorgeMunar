using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace carSales_app3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }


        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {

            // error of not valid information customerName, customerPhone

            if (customerNameTextBox.Text == "")
            {

                var dialogMessage = new MessageDialog("Error! Please enter the customer name");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            if (customerPhoneTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Error! Please enter a valid phone number");
                await dialogMessage.ShowAsync();
                customerPhoneTextBox.Focus(FocusState.Programmatic);
                customerPhoneTextBox.SelectAll();
                return;

            }

            //Disable customerNameTextBox, customerPhoneTextBox, 

            customerNameTextBox.IsEnabled = false;

            customerPhoneTextBox.IsEnabled = false;

            // focus in vehiclePriceTextBox
            vehiclePriceTextBox.Focus(FocusState.Programmatic);
        }

        private async void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            const decimal GST_RATE = 0.1m;
            decimal vehiclePrice;
            decimal lessTradeIn;
            decimal subAmount;
            decimal GSTAmount;
            decimal finalAmount;

            //try and catch decimal, display error message 
            //set focus and select all in the textbox
            try
            {
                vehiclePrice = decimal.Parse(vehiclePriceTextBox.Text);
            }
            catch (Exception theExeption)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a valid price" + theExeption.Message);
                await dialogMessage.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                vehiclePriceTextBox.SelectAll();
                return;
            }

            // vehicle price not empty

            if (vehiclePriceTextBox.Text == "")
            {

                var dialogMessage = new MessageDialog("Error! Please enter the vehicle price");
                await dialogMessage.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                return;
            }

            //try and catch decimal, display error message set focus and select all in the textbox
            // focus and select in lessTradeInTextBox            


            try
            {
                lessTradeIn = decimal.Parse(lessTradeInTextBox.Text);
            }
            catch (Exception theExeption)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a valid less trade in price" + theExeption.Message);
                await dialogMessage.ShowAsync();
                lessTradeInTextBox.Focus(FocusState.Programmatic);
                lessTradeInTextBox.SelectAll();
                return;
            }

            // less trade in value not empty

            if (lessTradeInTextBox.Text == "")
            {

                var dialogMessage = new MessageDialog("Error! Please enter the less trade value");
                await dialogMessage.ShowAsync();
                lessTradeInTextBox.Focus(FocusState.Programmatic);
                return;
            }

            //Check that vehicle price < lessTradeIn.

            if (vehiclePrice < lessTradeIn)
            {
                var dialogMessage = new MessageDialog("Invalid data, the Vehicle price cannot be less than the Trade In value");
                await dialogMessage.ShowAsync();
                return;
            }


            //Calculate Sub amount, GST amount and final amount 

            subAmount = vehiclePrice - lessTradeIn;
            subAmountTextBox.Text = subAmount.ToString("C");

            GSTAmount = subAmount * GST_RATE;
            GSTAmountTextBox.Text = GSTAmount.ToString("C");


            finalAmount = subAmount + GSTAmount;
            finalAmountTextBox.Text = finalAmount.ToString("C");

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear all the fields and focus in customerNameTextBox;

            customerNameTextBox.Text = "";
            customerPhoneTextBox.Text = "";
            vehiclePriceTextBox.Text = "";
            lessTradeInTextBox.Text = "";
            vehicleMakeTextBox.Text = "";
            subAmountTextBox.Text = "";
            GSTAmountTextBox.Text = "";
            finalAmountTextBox.Text = "";


            //Disable customerNameTextBox, customerPhoneTextBox, 

            customerNameTextBox.IsEnabled = true;
            customerPhoneTextBox.IsEnabled = true;

            // customerNameTextBox text focus

            customerNameTextBox.Focus(FocusState.Programmatic);
            return;
        }

        // Arrays

        // names array

        private string[] nameArray = new string[] { "Fredy", "Andres", "Jorge", "Enrique",
                "Liliana", "Marcela", "Jose", "Jesus", "Blanca", "Elsy"};

        // phone number array
        private int[] phoneNumberArray = new int[] {0414123450, 0414123451, 0414123452, 0414123453,
                0414123454, 0414123455, 0414123456, 0414123457, 0414123458, 0414123459};

        // vehicles array

        private string[] vehicleMakesArray = new string[] { "Toyota", "Holden", "Mitsubishi",
            "Ford", "BMW", "Mazda" };


        private async void searchNameButton_Click(object sender, RoutedEventArgs e)
        {

            string name = customerNameTextBox.Text;

            // error if name is empty

            if (name == "")
            {

                var dialogMessage = new MessageDialog("Error! Please enter the customer's name");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }


            // name sequencial search

            int counter = 0;
            bool found = false;

            // customer name not empty


            string criteria = customerNameTextBox.Text.ToLower();

            while (!found && counter < nameArray.Length)
            {
                if (criteria == nameArray[counter].ToLower())
                    found = true;
                else
                    counter++;
            }

            // found or not found
            if (found)
            {
                nameAndPhoneTextBlock.Text = criteria + "  ";
            }
            else
            {
                var dialogMessage = new MessageDialog("The customer was not found");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                return;
            }

        }

        private void displayAllCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            string outputName = "";
            string outputPhone = "";

            for (int i = 0; i < nameArray.Length; i++)
            {
                outputName = outputName + outputPhone +"\n" + 
                    nameArray[i] + phoneNumberArray[i] + "   " + "\n";
            }
                        
            nameAndPhoneTextBlock.Text = "Name and Phone: \n" + 
                outputName + " " + outputPhone + "\n" + outputPhone;
        }

        private void displayAllMakesButton_Click(object sender, RoutedEventArgs e)
        {
            {
                string output = "";
               for (int i = 1; i < vehicleMakesArray.Length; i++)
                {
                    output = output + vehicleMakesArray[i] + "\n";
        //            Array.Sort(vehicleMakesArray, (x, y) => String.Compare(x.output, y.output));
                }
                vehicleMakesTextBlock.Text = "Vehicle Makes: \n" + output;
            }
        }

        private async void insertMakeButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;
            bool found = false;

            if (vehicleMakeTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Please enter a vehicle Make");
                await dialogMessage.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                return;
            }

            string criteria = vehicleMakeTextBox.Text.ToUpper();

            while (!found && counter < vehicleMakesArray.Length) 
            {
                if (criteria == vehicleMakesArray[counter].ToUpper()) 
                    found = true;
                else
                    counter++;  
            }
            if (found)    
            {
                var dialogMessage = new MessageDialog("The Vehicle Make already exist.");
                await dialogMessage.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                return;
            }
            else
            {
                Array.Resize(ref vehicleMakesArray, vehicleMakesArray.Length + 1);  
                
                vehicleMakesArray[vehicleMakesArray.Length - 1] = vehicleMakeTextBox.Text;
            }

            {
                string output = "";
                for (int i = 1; i < vehicleMakesArray.Length; i++)
                {
                    output = output + vehicleMakesArray[i] + "\n";
        //            Array.Sort(vehicleMakesArray, (x, y) => String.Compare(x.output, y.output));
                }

                vehicleMakesTextBlock.Text = "Vehicle Makes: \n" + output;
            }
            

        }
    }

}
