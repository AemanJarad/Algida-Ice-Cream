using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Algida_Ice_Cream.IceCreamHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Algida_Ice_Cream
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<CheckBox, (int row, int column)> checkBoxMap;
        private List<Order> orders = new List<Order>();  // Define orders list here

        public MainWindow()
        {
            InitializeComponent();
            checkBoxMap = new Dictionary<CheckBox, (int row, int column)>
            {
                { IceLemonCheckBox, (0, 0) },
                { IceBananaCheckBox, (1, 0) },
                { IceStrawberryCheckBox, (2, 0) },
                { IceChocolateCheckBox, (3, 0) }
            };

            foreach (var checkBox in checkBoxMap.Keys)
            {
                checkBox.Checked += CheckBox_Checked;
            }

            // Set the DataGrid's ItemsSource to the list of orders
            InvoiceDataGrid.ItemsSource = orders;
        }


        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update UI element properties based on text changes

            UIElement[] elementsToEnable = new UIElement[] { GroupTypeOfMilk, GroupAdditions, GroupExtra, InsertButton, NewOrderButton, DeleteButton, GroupAmount };
            IceCreamHelper.UpdateElementProperties(NameTextBox, AddressTextBox, elementsToEnable);

            // Display a message in txtBlkName based on text length

            if (NameTextBox.Text.Length > 0)
            {
                txtBlkName.Text = string.Empty;
            }
            else
            {
                txtBlkName.Text = "Enter Your Name";
            }

        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update UI element properties based on text changes

            UIElement[] elementsToEnable = new UIElement[] { GroupTypeOfMilk, GroupAdditions, GroupExtra, InsertButton, NewOrderButton, DeleteButton, GroupAmount }; 
            IceCreamHelper.UpdateElementProperties(NameTextBox, AddressTextBox, elementsToEnable);

            // Display a message in txtBlkAddress based on text length

            if (AddressTextBox.Text.Length > 0)
            {
                txtBlkAddress.Text = string.Empty;
            }
            else
            {
                txtBlkAddress.Text = "Enter Your Address";
            }
            // Convert the entered name to title case
            NameTextBox.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NameTextBox.Text.ToLower());
        }


        private void NumericUpDownButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract information from UI and update total calories, price, and images
            Button button = (Button)sender;
            StackPanel stackPanel = (StackPanel)button.Parent;
            TextBox textBox = (TextBox)stackPanel.Children[1];

            if (button.Content.ToString() == "+")
            {
                IceCreamHelper.NumericUpDown.IncreaseValue(textBox);
            }
            else
            {
                IceCreamHelper.NumericUpDown.DecreaseValue(textBox);
            }
            UpdateTotalCaloriesAndPrice();
            double TotalPrice = Convert.ToDouble(UnitPriceTextBox.Text); // Update the total price based on quantity
            int Amount = Convert.ToInt32(AmountTextBox.Text);
            TotalPrice = TotalPrice * Amount ;
            TotalPriceTextBox.Text = TotalPrice.ToString();

            IceCreamHelper.UpdateImageVisibility(IceLemonCheckBox, IceLemonTextBox, ImgLimon);
            IceCreamHelper.UpdateImageVisibility(IceBananaCheckBox, IceBananaTextBox, ImgBanana);
            IceCreamHelper.UpdateImageVisibility(IceStrawberryCheckBox, IceStrowbTextBox, ImgStrawberry);
            IceCreamHelper.UpdateImageVisibility(IceChocolateCheckBox, IceChockTextBox, ImgChocolate);


        }
        

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Update total calories, price, and images based on checkbox state
            UpdateTotalCaloriesAndPrice();
            IceCreamHelper.UpdateImageVisibility(IceLemonCheckBox, IceLemonTextBox, ImgLimon);
            IceCreamHelper.UpdateImageVisibility(IceBananaCheckBox, IceBananaTextBox, ImgBanana);
            IceCreamHelper.UpdateImageVisibility(IceStrawberryCheckBox, IceStrowbTextBox, ImgStrawberry);
            IceCreamHelper.UpdateImageVisibility(IceChocolateCheckBox, IceChockTextBox, ImgChocolate);

        }

        private float[,] floatArray;
        // Update total calories and price based on selected options
        private void UpdateTotalCaloriesAndPrice() // ******
        {
            // Define a float array based on the selected milk type

            if (DietButton.IsChecked == true)
            {
                floatArray = new float[4, 2]
                {
                    { 115f, 0.25f },
                    { 175f, 0.55f },
                    { 135f, 0.75f },
                    { 235f, 0.80f }
                };
            }
            else if (LowFatButton.IsChecked == true)
            {
                floatArray = new float[4, 2]
                {
                    { 125f, 0.35f },
                    { 325f, 0.65f },
                    { 225f, 0.85f },
                    { 340f, 0.90f }
                };
            }
            else
            {
                floatArray = new float[4, 2]
                {
                    { 175f, 0.25f },
                    { 365f, 0.55f },
                    { 280f, 0.75f },
                    { 400f, 0.80f }
                };
            }

            // Calculate the total calorie and price based on selected checkboxes
            float totalCalories = 0f;
            float totalPrice = 0f;

            foreach (var kvp in checkBoxMap)
            {
                CheckBox chk = kvp.Key;
                (int r, int c) = kvp.Value;

                if (chk.IsChecked == true)
                {
                    float quantity = GetQuantity(chk); // Implement GetQuantity based on your UI
                    totalCalories += floatArray[r, c] * quantity;
                    totalPrice += floatArray[r, c + 1] * quantity;
                }
            }

            // Now you have the total calorie and price for the selected flavors and quantities
            UnitCaloriTextBox.Text = totalCalories.ToString();
            UnitPriceTextBox.Text = totalPrice.ToString();
        }

        
        private float GetQuantity(CheckBox checkBox) // Get the quantity from the UI based on checkbox name
        {
            string quantityText = GetQuantityFromUI(checkBox.Name); // Pass the name of the checkbox.
            if (float.TryParse(quantityText, out float quantity))
            {
                return quantity;
            }
            else
            {
                // return a default value
                return 1f;
            }
        }

        private string GetQuantityFromUI(string checkBoxName) // Get quantity from UI based on checkbox name
        {
            // Implement this method to get quantity from your UI based on the CheckBox name.
            // You can access the corresponding TextBox using the CheckBox name.
            switch (checkBoxName)
            {
                case "IceLemonCheckBox":
                    return IceLemonTextBox.Text;
                case "IceBananaCheckBox":
                    return IceBananaTextBox.Text;
                case "IceStrawberryCheckBox":
                    return IceStrowbTextBox.Text;
                case "IceChocolateCheckBox":
                    return IceChockTextBox.Text;
                default:
                    return "0";
            }
        }




        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Move focus to the next UI element on Enter key press
            if (e.Key == Key.Enter)
            {
                NameTextBox.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NameTextBox.Text.ToLower());
                var element = sender as UIElement;
                if (element != null)
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
        }

        private void PeaNutButton_Checked(object sender, RoutedEventArgs e) // Update total calories and add calories for PeaNut
        {
            
            UpdateTotalCaloriesAndPrice(); 
            int PeaNut = Convert.ToInt32(UnitCaloriTextBox.Text);
            PeaNut += 30;
            UnitCaloriTextBox.Text=PeaNut.ToString();

        }

        private void HazelNutButton_Checked(object sender, RoutedEventArgs e) // Update total calories and add calories for HazelNut
        {
            UpdateTotalCaloriesAndPrice();
            int HazelNut = Convert.ToInt32(UnitCaloriTextBox.Text);
            HazelNut += 45;
            UnitCaloriTextBox.Text = HazelNut.ToString();
        }

        private void AntepNutButton_Checked(object sender, RoutedEventArgs e) // Update total calories and add calories for AntepNut
        {
            UpdateTotalCaloriesAndPrice();
            int AntepNut = Convert.ToInt32(UnitCaloriTextBox.Text);
            AntepNut += 75;
            UnitCaloriTextBox.Text = AntepNut.ToString();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e) 
        {
            // Your existing code to get the order details
            // Get the order details from the UI
            string Name = NameTextBox.Text;
            string Address = AddressTextBox.Text;
            string TypeOfMilk = IceCreamHelper.GetSelectedMilk(GroupTypeOfMilk);
            List<string> selectedFlavors = IceCreamHelper.GetSelectedFlavors(StackAdditions);
            string Extra = IceCreamHelper.GetSelectedExtra(StackExtra);
            string UnitPrice = UnitPriceTextBox.Text;
            string UnitCalori = UnitCaloriTextBox.Text;
            string Amount = AmountTextBox.Text;
            string TotalPrice = TotalPriceTextBox.Text;

            // Create a new Order object with the extracted details
            Order newOrder = new Order
            {
                Name = Name,
                Address = Address,
                TypeOfMilk = TypeOfMilk,
                SelectedFlavors = string.Join(", ", selectedFlavors),
                Extra = Extra,
                UnitPrice = UnitPrice,
                UnitCalori = UnitCalori,
                Amount = Amount,
                TotalPrice = TotalPrice
            };

            // Retrieve the existing list of orders from the DataGrid's ItemsSource
            var orders = InvoiceDataGrid.ItemsSource as List<Order>;

            // Add the new order to the list
            orders.Add(newOrder);

            int itemCount = InvoiceDataGrid.Items.Count;

            // Update the TextBox with the item count
            GrandTotalTextBox.Text = itemCount.ToString();
            // Refresh the DataGrid to reflect the changes
            InvoiceDataGrid.Items.Refresh();
        }

        private void InvoiceDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = InvoiceDataGrid.SelectedItems.Count > 0;
            // Get the number of items in the DataGrid
            int itemCount = InvoiceDataGrid.Items.Count;

            // Update the TextBox with the item count
            GrandTotalTextBox.Text = itemCount.ToString();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected items
            List<Order> selectedOrders = InvoiceDataGrid.SelectedItems.Cast<Order>().ToList();

            // Remove the selected items from your data source
            foreach (var order in selectedOrders)
            {
                orders.Remove(order);
            }

            // Refresh the DataGrid
            InvoiceDataGrid.Items.Refresh();
        }

        private void NewOrderButton_Click(object sender, RoutedEventArgs e) // Clear UI elements for a new order
        {
            // Clear Type of Milk radio buttons
            IceCreamHelper.ClearTypeOfMilk(GroupTypeOfMilk);


            // Clear Additions checkboxes and related textboxes
            IceCreamHelper.ClearAdditions(StackAdditions);

            // Clear Extra radio buttons
            IceCreamHelper.ClearExtra(StackExtra);

            // Clear textboxes for price, calories, and amount
            
            
            AmountTextBox.Text = "1";
            
            IceCreamHelper.ClearTextBoxes(UnitPriceTextBox, UnitCaloriTextBox, TotalPriceTextBox);
            ImgLimon.Visibility = Visibility.Hidden;
            ImgBanana.Visibility = Visibility.Hidden;
            ImgStrawberry.Visibility = Visibility.Hidden;
            ImgChocolate.Visibility = Visibility.Hidden;


        }

        private void NewCustomer_Click(object sender, RoutedEventArgs e) // Clear UI elements for a new customer
        {
            IceCreamHelper.ClearTypeOfMilk(GroupTypeOfMilk);
            IceCreamHelper.ClearAdditions(StackAdditions);
            IceCreamHelper.ClearExtra(StackExtra);
            AmountTextBox.Text = "1";
            IceCreamHelper.ClearTextBoxes(UnitPriceTextBox, UnitCaloriTextBox, TotalPriceTextBox, NameTextBox, AddressTextBox);

            ImgLimon.Visibility = Visibility.Hidden;
            ImgBanana.Visibility = Visibility.Hidden;
            ImgStrawberry.Visibility = Visibility.Hidden;
            ImgChocolate.Visibility = Visibility.Hidden;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Display a confirmation dialog before exiting
            MessageBoxResult dialog = MessageBox.Show("Do you want to Exit", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dialog == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

    }

}
